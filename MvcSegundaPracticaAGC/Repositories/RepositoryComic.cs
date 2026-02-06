using Microsoft.Data.SqlClient;
using MvcSegundaPracticaAGC.Models;
using System.Data;

namespace MvcSegundaPracticaAGC.Repositories
{
    public class RepositoryComic
    {
        private readonly SqlConnection cn;
        private readonly SqlCommand com;
        private readonly DataTable tableComics;

        public RepositoryComic()
        {
            string stringConnection = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=COMICS;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";

            this.cn = new SqlConnection(stringConnection);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "SELECT * FROM Comics";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);

            this.tableComics = new DataTable();
            ad.Fill(this.tableComics);

        }

        public List<Comics> GetComics()
        {
            var consulta = from datos in this.tableComics.AsEnumerable()
                           select new Comics
                           {
                               IdComic = datos.Field<int>("IDCOMIC"),
                               Nombre = datos.Field<string>("NOMBRE"),
                               Imagen = datos.Field<string>("IMAGEN"),
                               Descripcion = datos.Field<string>("DESCRIPCION"),
                           };

            return consulta.ToList();
        }

        public Comics FindComic(int idComic)
        {

            var consulta = from datos in this.tableComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idComic
                           select new Comics
                           {
                               IdComic = datos.Field<int>("IDCOMIC"),
                               Nombre = datos.Field<string>("NOMBRE"),
                               Imagen = datos.Field<string>("IMAGEN"),
                               Descripcion = datos.Field<string>("DESCRIPCION"),
                           };
            return consulta.First();

        }

        public async Task CreateComic(Comics comics)
        {
            var consulta = from datos in this.tableComics.AsEnumerable()
                           select datos;

            var idComic = consulta.Max(i => i.Field<int>("IDCOMIC")+1);

            var sql="insert into Comics values(@idComic,@Nombre,@Imagen,@Descripcion)";

            this.com.Parameters.AddWithValue("@IDCOMIC",idComic);
            this.com.Parameters.AddWithValue("Nombre",comics.Nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", comics.Imagen);
            this.com.Parameters.AddWithValue("@DESCRIPCION", comics.Descripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
