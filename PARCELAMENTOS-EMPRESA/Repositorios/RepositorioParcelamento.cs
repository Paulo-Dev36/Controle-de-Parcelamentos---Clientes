using FirebirdSql.Data.FirebirdClient;
using Microsoft.Office.Interop.Excel;
using Npgsql;
using PARCELAMENTOS_EMPRESA.Classes;
using Projeto_Construir_Desktop;
using Projeto_Construir_Desktops;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace PARCELAMENTOS_EMPRESA.Repositorios
{
    public class RepositorioParcelamento : RepositorioAbstrato<ParcelamentosEmpresa>
    {
        private ConexaoFDB conexaoFDB = new ConexaoFDB();
        private RepositorioEmpresa repositorioEmpresa = new RepositorioEmpresa();
        private RepositorioUsuario repositorioUsuario = new RepositorioUsuario();

        public int QuantidadeEmpresas { get; set; }
        public List<ParcelamentosEmpresa> ParcelamentosEmpresas { get; set; }

        public override void Add(ParcelamentosEmpresa parcelamentosEmpresa)
        {
            NpgsqlConnection connect = conexaoFDB.ConexaoBanco();
            connect.Open();
            var insert = @"INSERT INTO parcelamentosempresa (idparcelamento, idempresa, cidade, atividade, regime, parcelamento, tipo, parcela, data, status, idusuario)
                                        VALUES(@Idparcelamento, @Idempresa, @Cidade, @Atividade, @Regime, @Parcelamento, @Tipo, @Parcela, @Data, @Status, @Idsuario)";

            var cmd = connect.CreateCommand();
            cmd.CommandText = insert;


            cmd.Parameters.AddWithValue(@"Idparcelamento", ObterUltimoId(connect));
            cmd.Parameters.AddWithValue(@"Idempresa", parcelamentosEmpresa.Empresa.First().Id);
            cmd.Parameters.AddWithValue(@"Cidade", parcelamentosEmpresa.Cidade);
            cmd.Parameters.AddWithValue(@"Atividade", parcelamentosEmpresa.Atividade);
            cmd.Parameters.AddWithValue(@"Regime", parcelamentosEmpresa.Regime);
            cmd.Parameters.AddWithValue(@"Parcelamento", parcelamentosEmpresa.Parcelamento);
            cmd.Parameters.AddWithValue(@"Tipo", parcelamentosEmpresa.Tipo);
            cmd.Parameters.AddWithValue(@"Parcela", parcelamentosEmpresa.Parcela);
            cmd.Parameters.AddWithValue(@"Data", parcelamentosEmpresa.Data);
            cmd.Parameters.AddWithValue(@"Status", parcelamentosEmpresa.Status);
            cmd.Parameters.AddWithValue(@"Idsuario", parcelamentosEmpresa.Usuario.First().Id);

            cmd.ExecuteNonQuery();
            connect.Close();
        }

        private int ObterUltimoId(NpgsqlConnection connect)
        {
            var obterUltimoId = @"SELECT * FROM parcelamentosempresa
                                    ORDER BY 1 DESC
                                    LIMIT 1 ";

            var cmd2 = connect.CreateCommand();
            cmd2.CommandText = obterUltimoId;

            var cmdDt2 = new NpgsqlDataAdapter(cmd2);
            var dataTable = new DataTable();
            cmdDt2.Fill(dataTable);
            return (int)dataTable.Rows[0][0] + 1;
        }

        public override IEnumerable<ParcelamentosEmpresa> Get(Expression<Func<ParcelamentosEmpresa, bool>> predicate)
        {
            var parcelamentosEmpresa = (List<ParcelamentosEmpresa>)GetAll();
            ParcelamentosEmpresas = parcelamentosEmpresa.AsQueryable().Where(predicate).ToList();
            return parcelamentosEmpresa.AsQueryable().Where(predicate).ToList();
        }

        public override IEnumerable<ParcelamentosEmpresa> GetAll()
        {
            string listaParcelamentos = "SELECT * FROM parcelamentosempresa";

            NpgsqlConnection connect = conexaoFDB.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = listaParcelamentos;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);

                List<ParcelamentosEmpresa> parcelamentosEmpresas = new List<ParcelamentosEmpresa>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ParcelamentosEmpresa parcelamentosEmpresa = new ParcelamentosEmpresa();

                    parcelamentosEmpresa.Id = (int)dataTable.Rows[i][0];
                    parcelamentosEmpresa.Empresa = repositorioEmpresa.Get(x => x.Id.Equals((int)dataTable.Rows[i][1])); //(Empresas)dataTable.Rows[i][1],
                    parcelamentosEmpresa.Cidade = dataTable.Rows[i][2].ToString();
                    parcelamentosEmpresa.Atividade = dataTable.Rows[i][3].ToString();
                    parcelamentosEmpresa.Regime = dataTable.Rows[i][4].ToString();
                    parcelamentosEmpresa.Parcelamento = dataTable.Rows[i][5].ToString();
                    parcelamentosEmpresa.Tipo = dataTable.Rows[i][6].ToString();
                    parcelamentosEmpresa.Parcela = dataTable.Rows[i][7].ToString();
                    parcelamentosEmpresa.Data = (DateTime)dataTable.Rows[i][8];
                    parcelamentosEmpresa.Usuario = repositorioUsuario.Get(x => x.Id.Equals((int)dataTable.Rows[i][9]));
                    parcelamentosEmpresa.Status = dataTable.Rows[i][10].ToString();

                    parcelamentosEmpresas.Add(parcelamentosEmpresa);
                }
                QuantidadeEmpresas = parcelamentosEmpresas.Count;
                ParcelamentosEmpresas = parcelamentosEmpresas;
                connect.Close();
                return parcelamentosEmpresas;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            finally
            {
                connect.Close();
            }
        }

        public override void Remove(ParcelamentosEmpresa parcelamentosEmpresa)
        {
            throw new NotImplementedException();
        }

        public override void Update(ParcelamentosEmpresa parcelamentosEmpresa)
        {
            NpgsqlConnection connect = conexaoFDB.ConexaoBanco();
            try
            {
                connect.Open();

                var update = $@"UPDATE parcelamentosempresa
                                    SET CIDADE = @Cidade, ATIVIDADE = @Atividade, REGIME = @Regime, Parcelamento= @Parcelamento, TIPO = @Tipo,
                                    PARCELA = @Parcela, Data = @Data, IDUSUARIO = @Idusuario, STATUS = @Status
                                    WHERE IDPARCELAMENTO = {parcelamentosEmpresa.Id}";

                var cmd = connect.CreateCommand();
                cmd.CommandText = update;

                cmd.Parameters.AddWithValue("@Cidade", parcelamentosEmpresa.Cidade);
                cmd.Parameters.AddWithValue("@Atividade", parcelamentosEmpresa.Atividade);
                cmd.Parameters.AddWithValue("@Regime", parcelamentosEmpresa.Regime);
                cmd.Parameters.AddWithValue("@Parcela", parcelamentosEmpresa.Parcela);
                cmd.Parameters.AddWithValue("@Parcelamento", parcelamentosEmpresa.Parcelamento);
                cmd.Parameters.AddWithValue("@Tipo", parcelamentosEmpresa.Tipo);
                cmd.Parameters.AddWithValue("@Data", parcelamentosEmpresa.Data);
                cmd.Parameters.AddWithValue("@Idusuario", parcelamentosEmpresa.Usuario.First().Id);
                cmd.Parameters.AddWithValue("@Status", parcelamentosEmpresa.Status);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connect.Close();
            }
        }

        public IEnumerable<ParcelamentosEmpresa> GetByParcela(string parcela)
        {
            NpgsqlConnection connect = conexaoFDB.ConexaoBanco();
            try
            {
                connect.Open();

                var selectParcela = $@"SELECT * FROM PARCELAMENTOSEMPRESA WHERE UPPER(PARCELA) LIKE UPPER('%{parcela}%')";
                var cmd = connect.CreateCommand();
                cmd.CommandText = selectParcela;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);

                List<ParcelamentosEmpresa> parcelamentosEmpresas = new List<ParcelamentosEmpresa>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var parcelamentoEmpresa = new ParcelamentosEmpresa()
                    {
                        Id = (int)dataTable.Rows[i][0],
                        Empresa = repositorioEmpresa.Get(x => x.Id.Equals((int)dataTable.Rows[i][1])),
                        Cidade = dataTable.Rows[i][2].ToString(),
                        Atividade = dataTable.Rows[i][3].ToString(),
                        Regime = dataTable.Rows[i][4].ToString(),
                        Parcelamento = dataTable.Rows[i][5].ToString(),
                        Tipo = dataTable.Rows[i][6].ToString(),
                        Parcela = dataTable.Rows[i][7].ToString(),
                        Data = (DateTime)dataTable.Rows[i][8],
                        Usuario = repositorioUsuario.Get(x => x.Id.Equals((int)dataTable.Rows[i][9])),
                        Status = dataTable.Rows[i][10].ToString(),
                    };
                    parcelamentosEmpresas.Add(parcelamentoEmpresa);
                }
                return parcelamentosEmpresas;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            finally
            {
                connect.Close();
            }

        } 
    }
}
