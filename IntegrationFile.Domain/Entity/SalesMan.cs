namespace IntegrationFiles.Domain.Entity
{
    public class SalesMan
    {
        public SalesMan(string code, string cpf, string name, decimal salary)
        {
            Code = code;
            Cpf = cpf;
            Name = name;
            Salary = salary;
        }

        public string Code { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
    }
}
