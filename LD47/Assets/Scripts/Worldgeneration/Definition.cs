namespace Worldgeneration
{
    [System.Serializable]
    public class Definition<T>
    {
        public string name;
        public T value;

        public Definition(string name, T value)
        {
            this.name = name;
            this.value = value;
        }
    }
}