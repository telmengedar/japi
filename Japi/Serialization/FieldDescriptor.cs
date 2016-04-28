namespace GoorooMania.Japi.Serialization {
    public class FieldDescriptor {
        public string Name { get; set; }
        public string Type { get; set; }

        public override string ToString() {
            return Name;
        }
    }
}