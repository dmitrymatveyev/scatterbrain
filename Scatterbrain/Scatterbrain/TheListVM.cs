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

namespace Scatterbrain
{
    public class TheListVM : INotifyPropertyChanged
    {
        public TheListVM()
        {
            var theList = TheListRepo.Read().GetAwaiter().GetResult();
            if(theList != null)
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
                if (self)
                {
                    return;
                }
                TheListRepo.Write(TheList).GetAwaiter().GetResult();
            };

            Delete = new Command<Subject>(s =>
            {
                try
                {
                    self = true;
                    var dep = TheList.Departments.First(d => string.Equals(d.Title, s.Department, StringComparison.InvariantCultureIgnoreCase));
                    if (dep.Subjects.Count == 1)
                    {
                        TheList.Departments.Remove(dep);
                        Departments.Remove(s.Department);
                        return;
                    }
                    dep.Subjects.Remove(s);
                    TheListRepo.Write(TheList).GetAwaiter().GetResult();
                }
                finally
                {
                    self = false;
                }
            });

            Add = new Command<Subject>(s =>
            {
                try
                {
                    self = true;
                    var dep = TheList.Departments.FirstOrDefault(d => string.Equals(d.Title, s.Department, StringComparison.InvariantCultureIgnoreCase));
                    if (dep == null)
                    {
                        dep = new Department { Title = s.Department };
                        TheList.Departments.Add(dep);
                        Departments.Add(s.Department);
                    }
                    dep.Subjects.Add(s);
                    TheListRepo.Write(TheList).GetAwaiter().GetResult();
                }
                finally
                {
                    self = false;
                }
            });
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
