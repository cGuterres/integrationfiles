namespace IntegrationFiles.Domain.Entity
{
    public class Customer
    {
        public Customer(string code, string cnpj, string name, string businessArea)
        {
            Code = code;
            Cnpj = cnpj;
            Name = name;
            BusinessArea = businessArea;
        }

        public string Code { get; set; }
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string BusinessArea { get; set; }
    }
}
