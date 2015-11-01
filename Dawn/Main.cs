using UnityEngine;

namespace Dawn
{
    public class Main : IMod
    {
        private GameObject _go;

        #region Implementation of IMod

        public void onEnabled()
        {
            _go = new GameObject(Name);
            _go.AddComponent<DawnController>();
        }

        public void onDisabled()
        {
            Object.Destroy(_go);
        }

        public string Name
        {
            get { return "Dawn"; }
        }

        public string Description
        {
            get { return "Adds Day-Night Cycle."; }
        }

        #endregion
    }
}