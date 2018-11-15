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
                var dep = TheList.Departments.First(d => d.Title == s.Department);
                if (dep.Subjects.Count == 1)
                {
                    TheList.Departments.Remove(dep);
                    return;
                }
                dep.Subjects.Remove(s);
            });

            Add = new Command<Subject>(s =>
            {
                var dep = TheList.Departments.FirstOrDefault(d => d.Title == s.Department);
                if (dep == null)
                {
                    dep = new Department { Title = s.Department };
                    TheList.Departments.Add(dep);
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

        public ICommand Delete { get; }

        public ICommand Add { get; }
    }
}
