using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRAC
{
    public class MovementWayPath
    {

        private bool add = false, remove = false, pathPicked = false, editPoint = false, editingPoint = false;
        private bool addPoint = false, addingPoint = false, overUnder = false, run = false, runFrom = false, skipLines = false;
        private bool stopSP = false, startSP=false;
        private int mode=1;
        private string path, fileName;
        private int indexLb = 0;

        #region GETSET
        public bool Add { get => add; set => add = value; }
        public bool Remove { get => remove; set => remove = value; }
        public bool PathPicked { get => pathPicked; set => pathPicked = value; }
        public string Path { get => path; set => path = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public int IndexLb { get => indexLb; set => indexLb = value; }
        public bool EditPoint { get => editPoint; set => editPoint = value; }
        public bool EditingPoint { get => editingPoint; set => editingPoint = value; }
        public bool AddPoint { get => addPoint; set => addPoint = value; }
        public bool AddingPoint { get => addingPoint; set => addingPoint = value; }
        public bool OverUnder { get => overUnder; set => overUnder = value; }
        public int Mode { get => mode; set => mode = value; }
        public bool Run { get => run; set => run = value; }
        public bool RunFrom { get => runFrom; set => runFrom = value; }
        public bool SkipLines { get => skipLines; set => skipLines = value; }
        public bool StopSP { get => stopSP; set => stopSP = value; }
        public bool StartSP { get => startSP; set => startSP = value; }
        #endregion
    }
}
