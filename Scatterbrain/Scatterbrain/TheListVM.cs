using Scatterbrain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Scatterbrain
{
    public class TheListVM : INotifyPropertyChanged
    {
        public TheListVM()
        {
            var dep1 = new Department
            {
                Title = "Department 1"
            };

            var sub1 = new Subject
            {
                Department = dep1.Title,
                Description = "Subject 1"
            };
            var sub2 = new Subject
            {
                Department = dep1.Title,
                Description = "Subject 2"
            };
            var sub3 = new Subject
            {
                Department = dep1.Title,
                Description = "Subject 3"
            };

            dep1.Subjects.Add(sub1);
            dep1.Subjects.Add(sub2);
            dep1.Subjects.Add(sub3);

            var dep2 = new Department
            {
                Title = "Department 2"
            };

            var sub4 = new Subject
            {
                Department = dep2.Title,
                Description = "Subject 4"
            };
            var sub5 = new Subject
            {
                Department = dep2.Title,
                Description = "Subject 5"
            };

            dep2.Subjects.Add(sub3);
            dep2.Subjects.Add(sub4);

            var dep3 = new Department
            {
                Title = "Department 3"
            };

            var sub6 = new Subject
            {
                Department = dep3.Title,
                Description = "Subject 6"
            };

            dep3.Subjects.Add(sub6);

            var theList = new TheList();
            theList.Departments.Add(dep1);
            theList.Departments.Add(dep2);
            theList.Departments.Add(dep3);

            TheList = theList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private TheList _theList;

        public TheList TheList
        {
            get { return _theList; }
            set
            {
                _theList = value;
                NotifyPropertyChanged();
            }
        }
    }
}
