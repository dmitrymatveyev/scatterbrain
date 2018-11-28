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
                theList.Departments.Select(d => d.Title).ToList().ForEach(Departments.Add);
                TheList = theList;
            }
            else
            {
                TheList = new TheList();
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
                            Departments.Remove(s.Department);
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
                            Departments.Add(s.Department);
                        }
                        if (dep.Subjects.Contains(s))
                        {
                            return;
                        }
                        dep.Subjects.Add(s);
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
    }
}
