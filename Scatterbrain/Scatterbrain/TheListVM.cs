using Scatterbrain.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ScatterbrainRepo = Scatterbrain.Data.ScatterbrainRepository;
using System.Threading.Tasks.Dataflow;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Scatterbrain
{
    public class TheListVM : INotifyPropertyChanged
    {
        public TheListVM()
        {
            InitTheList();
            InitDeps();
        }

        private void OnUI(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        private void InitTheList()
        {
            var theList = ScatterbrainRepo.GetSubjects().GetAwaiter().GetResult();
            if (theList != null)
            {
                TheList = theList;
            }
            else
            {
                TheList = new TheList();
            }

            TheList.Departments.CollectionChanged += TheListDepsChanged;
        }

        private TheList _theList;
        public TheList TheList
        {
            get { return _theList; }
            private set
            {
                _theList = value;
                NotifyPropertyChanged();
            }
        }

        private void InitDeps()
        {
            var deps = ScatterbrainRepo.GetDepartments().GetAwaiter().GetResult();
            if(deps != null)
            {
                Deps = deps;
            }
            else
            {
                Deps = new ObservableCollection<string>();
            }
        }

        private bool _isSelfUpdatingDeps;
        private ObservableCollection<string> _deps;
        public ObservableCollection<string> Deps
        {
            get { return _deps; }
            set
            {
                _deps = value;
                NotifyPropertyChanged();
            }
        }

        private void TheListDepsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
            }

            var isDepsChanged = false;
            foreach (var dep in e.NewItems.Cast<Department>())
            {
                if (!Deps.Contains(dep.Title))
                {
                    Deps.Add(dep.Title);
                    isDepsChanged = true;
                }
            }
            if (isDepsChanged)
            {
                ScatterbrainRepo.StoreDepartments(Deps).GetAwaiter().GetResult();
            }

            if (_isSelfUpdatingDeps)
            {
                return;
            }
            ScatterbrainRepo.StoreSubjects(TheList).GetAwaiter().GetResult();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ICommand _delSubj;
        public ICommand DelSubj
        {
            get
            {
                if (_delSubj != null)
                {
                    return _delSubj;
                }

                var block = new ActionBlock<Subject>(s =>
                {
                    var dep = TheList.Departments.FirstOrDefault(d => string.Equals(d.Title, s.Department, StringComparison.InvariantCultureIgnoreCase));
                    if (dep == null)
                    {
                        return;
                    }
                    OnUI(() =>
                    {
                        try
                        {
                            _isSelfUpdatingDeps = true;
                            if (dep.Subjects.Count == 1)
                            {
                                TheList.Departments.Remove(dep);
                            }
                            else
                            {
                                if (!dep.Subjects.Contains(s))
                                {
                                    return;
                                }
                                dep.Subjects.Remove(s);
                            }
                            ScatterbrainRepo.StoreSubjects(TheList);
                        }
                        finally
                        {
                            _isSelfUpdatingDeps = false;
                        }
                    });
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

                return _delSubj = new Command<Subject>(s => block.Post(s));
            }
        }

        private ICommand _addSubj;
        public ICommand AddSubj
        {
            get
            {
                if (_addSubj != null)
                {
                    return _addSubj;
                }

                var block = new ActionBlock<Subject>(s =>
                {
                    var dep = TheList.Departments.FirstOrDefault(d => string.Equals(d.Title, s.Department, StringComparison.InvariantCultureIgnoreCase));
                    OnUI(() =>
                    {
                        try
                        {
                            _isSelfUpdatingDeps = true;
                            if (dep == null)
                            {
                                dep = new Department { Title = s.Department };
                                TheList.Departments.Add(dep);
                            }
                            if (dep.Subjects.Contains(s))
                            {
                                return;
                            }
                            dep.Subjects.Add(s);
                            ScatterbrainRepo.StoreSubjects(TheList);
                        }
                        finally
                        {
                            _isSelfUpdatingDeps = false;
                        }
                    });
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

                return _addSubj = new Command<Subject>(s => block.Post(s));
            }
        }

        private ICommand _delDep;
        public ICommand DelDep
        {
            get
            {
                if (_delDep != null)
                {
                    return _delDep;
                }

                var block = new ActionBlock<string>(d =>
                {
                    OnUI(() =>
                    {
                        Deps.Remove(d);
                        ScatterbrainRepo.StoreDepartments(Deps);
                    });
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

                return _delDep = new Command<string>(d => block.Post(d));
            }
        }

        private int _busyCount;
        public bool IsBusy
        {
            get { return _busyCount > 0; }
            set
            {
                if (value)
                {
                    _busyCount++;
                }
                else if(_busyCount > 0)
                {
                    _busyCount--;
                }
                NotifyPropertyChanged();
            }
        }
    }
}
