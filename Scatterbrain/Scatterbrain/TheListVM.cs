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
using TheListRepo = Scatterbrain.Data.TheListRepository;
using System.Threading.Tasks.Dataflow;

namespace Scatterbrain
{
    public class TheListVM : INotifyPropertyChanged
    {
        public TheListVM()
        {
            var theList = TheListRepo.Read().GetAwaiter().GetResult();
            if (theList != null)
            {
                TheList = theList;
            }
            else
            {
                TheList = new TheList();
            }

            var departments = TheListRepo.ReadDepartments().GetAwaiter().GetResult();
            if(departments != null)
            {
                departments.ForEach(Departments.Add);
            }

            var self = false;

            TheList.Departments.CollectionChanged += (s, e) =>
            {
                if (self || e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    return;
                }
                TheListRepo.Write(TheList).GetAwaiter().GetResult();
            };

            var deleteBlock = new ActionBlock<Subject>(s =>
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
                        self = true;
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
                        TheListRepo.Write(TheList);
                    }
                    finally
                    {
                        self = false;
                    }
                });
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
            Delete = new Command<Subject>(s => deleteBlock.Post(s));

            var addBlock = new ActionBlock<Subject>(s =>
            {
                var dep = TheList.Departments.FirstOrDefault(d => string.Equals(d.Title, s.Department, StringComparison.InvariantCultureIgnoreCase));
                OnUI(() =>
                {
                    try
                    {
                        self = true;
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
                        if (!Departments.Contains(s.Department))
                        {
                            Departments.Add(s.Department);
                            TheListRepo.WriteDepartments(Departments);
                        }
                        TheListRepo.Write(TheList);
                    }
                    finally
                    {
                        self = false;
                    }
                });
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
            Add = new Command<Subject>(s => addBlock.Post(s));

            var deleteDepBlock = new ActionBlock<string>(d =>
            {
                OnUI(() =>
                {
                    Departments.Remove(d);
                    TheListRepo.WriteDepartments(Departments);
                });
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
            DeleteDepartment = new Command<string>(d => deleteDepBlock.Post(d));
        }

        private void OnUI(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public TheList TheList { get; }

        public ObservableCollection<string> Departments { get; } = new ObservableCollection<string>();

        public ICommand Delete { get; }

        public ICommand Add { get; }

        public ICommand DeleteDepartment { get; }
    }
}
