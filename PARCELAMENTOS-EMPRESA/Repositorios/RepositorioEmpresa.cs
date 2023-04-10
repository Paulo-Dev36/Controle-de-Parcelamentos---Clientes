using Npgsql;
using PARCELAMENTOS_EMPRESA.Classes;
using Projeto_Construir_Desktop;
using Projeto_Construir_Desktops;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace PARCELAMENTOS_EMPRESA.Repositorios
{   
    public class RepositorioEmpresa : RepositorioAbstrato<Empresas>
    {
        private ConexaoFDB conexaoFDB = new ConexaoFDB();
        public override void Add(Empresas x)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Empresas> Get(Expression<Func<Empresas, bool>> predicate)
        {
            var empresas = (List<Empresas>)GetAll();
            return empresas.AsQueryable().Where(predicate).ToList();
        }

        public override IEnumerable<Empresas> GetAll()
        {
            string listaUsuariosWL = @"SELECT EMPRESAS.IDEMPRESA, EMPRESAS.CODIGOEMPRESA, EMPRESAS.NOMEEMPRESA, EMPRESAS.EMAIL, 
                                        PRESTACAOSERVICO.STATUS, EMPRESAS.DDDFONE, EMPRESAS.CNPJ, EMPRESAS.EMAILSECUNDARIO, 
                                        EMPRESAS.MUNICIPIO, EMPRESAS.FILIAL, EMPRESAS.TIPO, EMPRESAS.INSCRICAOMUNICIPAL,
                                        EMPRESAS.TIPOEMPRESA, EMPRESAS.ENQUADRAMENTO
                                        FROM EMPRESAS       
                                        INNER JOIN PRESTACAOSERVICO ON PRESTACAOSERVICO.IDEMPRESA = EMPRESAS.IDEMPRESA";

            NpgsqlConnection connect = conexaoFDB.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = listaUsuariosWL;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);

                List<Empresas> empresas = new List<Empresas>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Empresas empresa = new Empresas();

                    empresa.Id = (int)dataTable.Rows[i][0];
                    empresa.Codigo = (int)dataTable.Rows[i][1];
                    empresa.NomeEmpresa = dataTable.Rows[i][2].ToString();
                    empresa.Email = dataTable.Rows[i][3].ToString();
                    empresa.Status = dataTable.Rows[i][4].ToString();
                    empresa.Cnpj = dataTable.Rows[i][6].ToString();
                    empresa.EmailSecundario = dataTable.Rows[i][7].ToString();
                    empresa.Municipio = dataTable.Rows[i][8].ToString();
                    empresa.Filial = (int)dataTable.Rows[i][9];
                    empresa.Tipo = dataTable.Rows[i][10].ToString();
                    empresa.InscricaoMunicipal = dataTable.Rows[i][11].ToString();
                    empresa.TipoEmpresa = dataTable.Rows[i][12].ToString();
                    empresa.Enquadramento = dataTable.Rows[i][13].ToString();

                    empresas.Add(empresa);
                }
                return empresas;
            }
            catch
            {
                return null;
            }
            finally 
            {
                connect.Close();
            }
        }

        public IEnumerable<Empresas> ObterEmpresaPorCodigoEFilial(int codigoEmpresa, int filial)
        {
            Empresas empresas = new Empresas();
            return Get(x => x.Codigo.Equals(codigoEmpresa) && x.Filial.Equals(filial));
        }
        public override void Remove(Empresas x)
        {
            throw new NotImplementedException();
        }

        public override void Update(Empresas x)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ListaNomeEmpresas()
        {
            IEnumerable<Empresas> empresas = GetAll().Where(x => x.Status == "Ativa").OrderBy(x => x.Codigo);
            List<string> nomeEmpresas = new List<string>();

            foreach (var empresa in empresas)
            {
                nomeEmpresas.Add(empresa.Codigo + "-" + empresa.Filial + "-" + empresa.NomeEmpresa);
            }

            return nomeEmpresas;
        }
    }
}
