namespace ProjectCore.Variables
{
    public interface IDBVariable
    {
        void SetKey(string key);
        string GetKey();
        
        void Update(object value);
        object GetValue();

        public virtual void Save()
        {
            
        }

        public virtual void Load()
        {
            
        }
    }
}