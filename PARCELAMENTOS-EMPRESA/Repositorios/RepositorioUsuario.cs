using Npgsql;
using Projeto_Construir_Desktop;
using Projeto_Construir_Desktops;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace PARCELAMENTOS_EMPRESA.Repositorios
{
    public class RepositorioUsuario : RepositorioAbstrato<Usuarios>
    {
        private ConexaoFDB conexaoFDB = new ConexaoFDB();
        public IEnumerable<Usuarios> Usuario;
        
        public override void Add(Usuarios x)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Usuarios> Get(Expression<Func<Usuarios, bool>> predicate)
        {
            var usuarios = (List<Usuarios>)GetAll();
            return usuarios.AsQueryable().Where(predicate).ToList();
        }

        public override IEnumerable<Usuarios> GetAll()
        {
            string listaUsuariosWL = "SELECT * FROM USUARIOS";

            NpgsqlConnection connect = conexaoFDB.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = listaUsuariosWL;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);

                List<Usuarios> usuarios = new List<Usuarios>();

                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Usuarios usuario = new Usuarios
                    {
                        Id = (int)dataTable.Rows[i][0],
                        NomeUsuario = dataTable.Rows[i][1].ToString(),
                        Senha = dataTable.Rows[i][2].ToString(),
                        Email = dataTable.Rows[i][3].ToString(),
                        Nome = dataTable.Rows[i][4].ToString(),
                        IdEmpresa = (int)dataTable.Rows[i][5],
                        Status = dataTable.Rows[i][7].ToString(),
                        NovaSenha = dataTable.Rows[i][12].ToString(),
                        AlterarSenha = (bool)dataTable.Rows[i][13]
                    };
                    usuarios.Add(usuario);
                }
                connect.Close();
                return usuarios;
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

        public override void Remove(Usuarios x)
        {
            throw new NotImplementedException();
        }

        public override void Update(Usuarios x)
        {
            throw new NotImplementedException();
        }

        public bool Logar(string nomeUsuario, string senhaUsuario,string senha)
        {
            var sql = $@"SELECT * FROM USUARIOS WHERE NOMEUSUARIO = '{nomeUsuario}' ";

            NpgsqlConnection connect = conexaoFDB.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = sql;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dtble = new DataTable();
                cmdDt.Fill(dtble);

                if (dtble.Rows.Count > 0)
                {
                    Usuarios usuario = new Usuarios
                    {
                        Id = (int)dtble.Rows[0][0],
                        NomeUsuario = dtble.Rows[0][1].ToString(),
                        Senha = dtble.Rows[0][2].ToString(),
                        Email = dtble.Rows[0][3].ToString(),
                        Nome = dtble.Rows[0][4].ToString(),
                        IdEmpresa = (int)dtble.Rows[0][5],
                        Status = dtble.Rows[0][7].ToString(),
                        NovaSenha = dtble.Rows[0][12].ToString(),
                        AlterarSenha = (bool)dtble.Rows[0][13]
                    };


                    if (usuario.Senha.Equals(senhaUsuario))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                connect.Close();
            }
            catch
            {
                return false;
            }
            finally 
            {
                connect.Close();
            }
            return false;
        }

        public IEnumerable<string> ListaNomeUsuariosWL()
        {
            IEnumerable<Usuarios> usuarios = GetAll().Where(x => x.IdEmpresa.Equals(74) && x.Status.Equals("Ativo"));
            List<string> nomesUsuarios = new List<string>();

            foreach (var usuario in usuarios)
            {
                nomesUsuarios.Add(usuario.NomeUsuario);
            }

            return nomesUsuarios;
        }

    }
}
