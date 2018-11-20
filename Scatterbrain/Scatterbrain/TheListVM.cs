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

namespace Scatterbrain
{
    public class TheListVM : INotifyPropertyChanged
    {
        public TheListVM()
        {
            Delete = new Command<Subject>(s =>
            {
                var dep = TheList.Departments.First(d => string.Equals(d.Title, s.Department, StringComparison.InvariantCultureIgnoreCase));
                if (dep.Subjects.Count == 1)
                {
                    TheList.Departments.Remove(dep);
                    Departments.Add(s.Department);
                    return;
                }
                dep.Subjects.Remove(s);
            });

            Add = new Command<Subject>(s =>
            {
                var dep = TheList.Departments.FirstOrDefault(d => string.Equals(d.Title, s.Department, StringComparison.InvariantCultureIgnoreCase));
                if (dep == null)
                {
                    dep = new Department { Title = s.Department };
                    TheList.Departments.Add(dep);
                    Departments.Add(s.Department);
                }
                dep.Subjects.Add(s);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public TheList TheList { get; } = new TheList();

        public ObservableCollection<string> Departments { get; } = new ObservableCollection<string>();

        public ICommand Delete { get; }

        public ICommand Add { get; }
    }
}
