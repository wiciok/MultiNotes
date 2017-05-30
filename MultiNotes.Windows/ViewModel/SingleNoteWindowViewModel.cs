using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MultiNotes.Model;
using MultiNotes.Windows.Annotations;

namespace MultiNotes.Windows.ViewModel
{

    //todo: po zmianie notatki trzeba wywolac update z noteapi!!!

    public class SingleNoteWindowViewModel : INotifyPropertyChanged
    {
        public SingleNoteWindowViewModel(Action<object> closeAction, Note note)
        {
            Note = note;
            _closeNoteAction = closeAction;
        }

        private readonly Action<object> _closeNoteAction;
        public Note Note { get; set; }

        public string DisplayedDate => Note.CreateTimestamp.ToShortDateString(); //todo: mozna to zmienic czy cos

        public ICommand CloseNoteCmd => new CommandHandler(_closeNoteAction);


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        
    }
}
