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
            var dep1 = "Department 1";

            var sub1 = new Subject
            {
                Department = dep1,
                Description = "Subject 1"
            };
            var sub2 = new Subject
            {
                Department = dep1,
                Description = "Subject 2"
            };
            var sub3 = new Subject
            {
                Department = dep1,
                Description = "Subject 3"
            };

            var dep2 = "Department 2";

            var sub4 = new Subject
            {
                Department = dep2,
                Description = "Subject 4"
            };
            var sub5 = new Subject
            {
                Department = dep2,
                Description = "Subject 5"
            };

            var dep3 = "Department 3";

            var sub6 = new Subject
            {
                Department = dep3,
                Description = "Subject 6"
            };

            var theList = new TheList();
            theList.Subjects.Add(sub1);
            theList.Subjects.Add(sub2);
            theList.Subjects.Add(sub3);
            theList.Subjects.Add(sub4);
            theList.Subjects.Add(sub5);
            theList.Subjects.Add(sub6);

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
