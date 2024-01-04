
using AppProfesores.Models;
using Microsoft.Data.SqlClient;

namespace AppProfesores.Data
{
    internal class Conexion
    {



        private static readonly string[] Querys =
        {
            "SELECT Nombre, horasP, horasA FROM Maestros",//0
            "SELECT materia,cred,gpo,l,m,m1,j,v FROM Materias",//1
            "SELECT TOP 1 materia, gpo, l, m, m1, j, v FROM Materias ORDER BY NEWID();",//2
            "SELECT top 1 Nombre FROM Maestros where horarioA=0 order by NEWID()",//3
            "update Maestros set horarioA=1 where nombre='{1}'",//4
            "SELECT * FROM Maestros where horarioA=0",//5
            "SELECT * from Horarios",//6
            "insert into Horarios values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",//7
            "select * from Horarios where profesor like '%{1}%';",//8
            "update Materias set profesor='{1}' where materia='{2}' and gpo = '{3}'",//9
            "SELECT TOP 1 materia, gpo, l, m, m1, j, v FROM Materias where Profesor='0' ORDER BY NEWID();",//10
            "select count(*) from Maestros where horarioA=0",//11
            "select count(*) from Materias where profesor='0'",//12
            "select nombre from Maestros where nombre like '%{1}%';",//13
            "select horasA from Maestros where nombre='{1}'",//14
            "update Maestros set horasA={1} where nombre='{2}'"//15

    };

        private static readonly string sCnn = $"Data Source=ALEX-MATEBOOK14\\SQLEXPRESS;Initial Catalog=Horarios; User Id=sa;Password=1234;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";

        public static ProfesoresModel consultarProfes(ListView vistaLista)
        {
            List<ProfesoresModel> lista = new List<ProfesoresModel>();
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmd = new SqlCommand(Querys[0], con);
            SqlDataReader r = cmd.ExecuteReader();

            ProfesoresModel model = new ProfesoresModel();



            if (r.HasRows)
            {
                while (r.Read())
                {
                    lista.Add(new ProfesoresModel
                    {
                        nombre = r["nombre"].ToString(),
                        horasA = r["horasA"].ToString(),
                        horasP = r["horasP"].ToString()
                    }
                    );
                }
                vistaLista.ItemsSource = lista;
            }
            r.Close();
            con.Close();
            return model;
        }
        public static MateriasModel consultarMates(ListView vistaLista)
        {
            List<MateriasModel> lista = new List<MateriasModel>();
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmd = new SqlCommand(Querys[1], con);
            SqlDataReader r = cmd.ExecuteReader();

            MateriasModel model = new MateriasModel();



            if (r.HasRows)
            {
                while (r.Read())
                {
                    lista.Add(new MateriasModel
                    {
                        nombreMaterias = r[0].ToString(),
                        cred = r["cred"].ToString(),
                        gpo = r["gpo"].ToString(),
                        l = r["l"].ToString(),
                        m = r["m"].ToString(),
                        m1 = r["m1"].ToString(),
                        j = r["j"].ToString(),
                        v = r["v"].ToString()
                    }
                    );
                }
                vistaLista.ItemsSource = lista;
            }
            r.Close();
            con.Close();
            return model;
        }

        public static MateriasModel consultarHorario(ListView vistaLista)
        {
            List<MateriasModel> lista = new List<MateriasModel>();
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmd = new SqlCommand(Querys[6], con);
            SqlDataReader r = cmd.ExecuteReader();

            MateriasModel model = new MateriasModel();



            if (r.HasRows)
            {
                while (r.Read())
                {
                    lista.Add(new MateriasModel
                    {
                        nombreMaterias = r["materia"].ToString(),
                        gpo = r["gpo"].ToString(),
                        l = r["l"].ToString(),
                        m = r["m"].ToString(),
                        m1 = r["m1"].ToString(),
                        j = r["j"].ToString(),
                        v = r["v"].ToString(),
                        profesor = r["profesor"].ToString(),
                    }
                    );
                }
                vistaLista.ItemsSource = lista;
            }
            r.Close();
            con.Close();
            return model;
        }

        public static void generarHorarios()
        {
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            //SqlCommand cmdG = new SqlCommand(Querys[5], con);//consulta de todos los profesores sin asignacion de horario
            //SqlDataReader rG = cmdG.ExecuteReader();
            int noEncontradas = 0;

            int mSin = 0;

            while (mSin != 167)
            {

                //SqlCommand cmdG = new SqlCommand(Querys[12], con);
                //SqlDataReader rG = cmdG.ExecuteReader();
                //rG.Read();
                //mSin = int.Parse(rG[0].ToString());
                System.Diagnostics.Debug.WriteLine(mSin);


                string profesor = obtenerProfe();
                int asignadas = 0;

                while (asignadas < 5 & noEncontradas < 50 & mSin != 167)
                {
                    SqlCommand cmdM = new SqlCommand(Querys[10], con);//1 materias aleatorias
                    SqlDataReader rM = cmdM.ExecuteReader();
                    rM.Read();
                    if (nochocanMaterias(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                    {
                        if (esConsecutiva(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                        {
                            if (entraenhorasAutorizadas(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                            { 
                                nonQuery(construyeQuery(7, profesor, rM["materia"].ToString(), rM["gpo"].ToString(), rM["l"].ToString(), rM["m"].ToString(), rM["m1"].ToString(), rM["j"].ToString(), rM["v"].ToString()));
                                nonQuery(construyeQuery(9, profesor, rM["materia"].ToString(), rM["gpo"].ToString()));
                                asignadas++;
                                mSin++;
                            }
                            else
                            {
                                noEncontradas++;
                            }
                        }
                        else
                        {
                            noEncontradas++;
                        }
                    }
                    else
                    {
                        noEncontradas++;
                    }

                    rM.Close();
                }
                noEncontradas = 0;
                nonQuery(construyeQuery(4, profesor));
            }
            //rG.Close();
            //con.Close();
        }
        public static void generarHorarioIndi()
        {

            SqlConnection con = new SqlConnection(sCnn);
            con.Open();

            int noEncontradas = 0;
            string profesor = obtenerProfe();
            int asignadas = 0;

            do
            {
                SqlCommand cmdM = new SqlCommand(Querys[2], con);//1 materias aleatorias
                SqlDataReader rM = cmdM.ExecuteReader();
                rM.Read();
                //System.Diagnostics.Debug.WriteLine(nochocanMaterias(profesor, rM[0].ToString(), rM[1].ToString()));
                if (nochocanMaterias(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                {
                    //System.Diagnostics.Debug.WriteLine(esConsecutiva(profesor, rM[0].ToString(), rM[1].ToString()));
                    if (esConsecutiva(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                    {
                        if (entraenhorasAutorizadas(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                        {
                            nonQuery(construyeQuery(7, profesor, rM["materia"].ToString(), rM["gpo"].ToString(), rM["l"].ToString(), rM["m"].ToString(), rM["m1"].ToString(), rM["j"].ToString(), rM["v"].ToString()));
                            nonQuery(construyeQuery(9, profesor, rM["materia"].ToString(), rM["gpo"].ToString()));
                            asignadas++;
                        }
                        else
                        {
                            noEncontradas++;
                        }
                    }
                    else
                    {
                        noEncontradas++;
                    }
                }
                else
                {
                    noEncontradas++;
                }
                rM.Close();
                System.Diagnostics.Debug.WriteLine(getHrsAsignadas(profesor)+","+horasAsignadas(profesor));
            } while (horasAsignadas(profesor) != getHrsAsignadas(profesor) & noEncontradas!=50);
            noEncontradas = 0;
            nonQuery(construyeQuery(4, profesor));

            con.Close();
        }
        public static void generarHorarioIndiDe(string prof)
        {

            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmdP = new SqlCommand(construyeQuery(13, prof), con);//obtención de profe aleatorio
            SqlDataReader rP = cmdP.ExecuteReader();
            rP.Read();
            string profesor = rP[0].ToString();
            rP.Close();
            int noEncontradas = 0;
            int asignadas = 0;

            while (asignadas < 5 & noEncontradas < 100)
            {
                SqlCommand cmdM = new SqlCommand(Querys[2], con);//1 materias aleatorias
                SqlDataReader rM = cmdM.ExecuteReader();
                rM.Read();
                if (nochocanMaterias(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                {
                    if (esConsecutiva(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                    {
                        if (entraenhorasAutorizadas(profesor, rM[0].ToString(), rM[1].ToString()) == true)
                        { 
                            nonQuery(construyeQuery(7, profesor, rM["materia"].ToString(), rM["gpo"].ToString(), rM["l"].ToString(), rM["m"].ToString(), rM["m1"].ToString(), rM["j"].ToString(), rM["v"].ToString()));
                            nonQuery(construyeQuery(9, profesor, rM["materia"].ToString(), rM["gpo"].ToString()));
                            asignadas++;
                        }
                        else
                        {
                            noEncontradas++;
                        }

                    }
                    else
                    {
                        noEncontradas++;
                    }
                }
                else
                {
                    noEncontradas++;
                }
                rM.Close();
            }
            noEncontradas = 0;
            nonQuery(construyeQuery(4, profesor));

            con.Close();
        }

        public static void eliminarHorarios()
        {
            nonQuery("delete from Horarios");
            nonQuery("update Maestros set horarioA=0");
            nonQuery("update Materias set Profesor='0'");
        }



        public static bool esConsecutiva(string profesor, string materia, string grupo)
        {
            bool entra = true;
            int[,] entradasSalidas = llenaMatrizEySyH(profesor);

            string newMateria = "select l, m, m1, j, v from Materias where materia='" + materia + "' and gpo='" + grupo + "'";
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmdG = new SqlCommand(newMateria, con);
            SqlDataReader readerNewMateria = cmdG.ExecuteReader();
            readerNewMateria.Read();
            for (int i = 0; i < entradasSalidas.GetLength(0); i++)
            {
                if (readerNewMateria[i].ToString() != "-")
                {
                    if (int.Parse(readerNewMateria[i].ToString()[..2]) == entradasSalidas[i, 1] || int.Parse(readerNewMateria[i].ToString()[3..]) == entradasSalidas[i, 0] & ((int.Parse(readerNewMateria[i].ToString()[3..]) - int.Parse(readerNewMateria[i].ToString()[..2])) + entradasSalidas[i, 2]) <= 8)
                    {
                        entra = true;

                    }
                    else if (entradasSalidas[i, 1] == 0 || entradasSalidas[i, 0] == 0)
                    {
                        entra = true;

                    }
                    else
                    {
                        entra = false;
                        break;
                    }
                }

            }
            readerNewMateria.Close();
            con.Close();

            //for (int i = 0; i < entradasSalidas.GetLength(0); i++)
            //{
            //    for (int j = 0; j < entradasSalidas.GetLength(1); j++)
            //    {
            //        System.Diagnostics.Debug.WriteLine(entradasSalidas[i, j]);
            //    }
            //    System.Diagnostics.Debug.WriteLine("\n");
            //}
            //System.Diagnostics.Debug.WriteLine(entra);
            return entra;
        }

        public static bool noExedeOchoHoras(string profesor, string materia, string grupo)
        {
            int horasxClase = 0;
            int[] horasxDias = new int[5];
            int[] horasNuevas = new int[5];
            string[,] horasExistentes = llenaMatrizdehoras(profesor);
            bool consecutiva = false;
            int horaInicio = 0, horaFinal = 0;

            for (int c = 0; c < horasExistentes.GetLength(1); c++)
            {
                for (int f = 0; f < horasExistentes.GetLength(0); f++)
                {
                    if (horasExistentes[f, c] != "-")
                    {
                        horaInicio = int.Parse(horasExistentes[f, c].ToString().Substring(0, 2));
                        horaFinal = int.Parse(horasExistentes[f, c].ToString().Substring(3));
                        horasxClase = horaFinal - horaInicio;
                    }
                    else
                    {
                        horasxClase = 0;
                    }
                    horasxDias[c] += horasxClase;
                }
            }

            string newMateria = "select l, m, m1, j, v from Materias where materia='" + materia + "' and gpo='" + grupo + "'";
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmdG = new SqlCommand(newMateria, con);
            SqlDataReader readerNewMateria = cmdG.ExecuteReader();
            readerNewMateria.Read();
            for (int i = 0; i < 5; i++)
            {
                if (readerNewMateria[i].ToString() != "-")
                {
                    horaInicio = int.Parse(readerNewMateria[i].ToString().Substring(0, 2));
                    horaFinal = int.Parse(readerNewMateria[i].ToString().Substring(3));
                    horasNuevas[i] = horaFinal - horaInicio;
                }
                else
                {
                    horasNuevas[i] = 0;
                }
            }
            readerNewMateria.Close();
            con.Close();
            for (int i = 0; i < 5; i++)
            {
                if ((horasxDias[i] + horasNuevas[i]) <= 8)
                {
                    consecutiva = true;
                }
                else
                {
                    consecutiva = false;
                    break;
                }
            }
            return consecutiva;
        }

        public static bool entraenhorasAutorizadas(string profesor, string materia, string grupo)
        {
            bool entra = false;
            int hrsAsignadas = horasAsignadas(profesor);
            int hrsxAsignar = 0;
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();

            int hrpagadas = getHrsAsignadas(profesor);

            SqlCommand cmdM = new SqlCommand("select l, m, m1, j, v from Materias where materia='" + materia + "' and gpo='" + grupo + "'", con);
            SqlDataReader readerMateria = cmdM.ExecuteReader();
            readerMateria.Read();
            for(int i = 0; i < 5; i++)
            {
                if (readerMateria[i].ToString() != "-")
                {
                    hrsxAsignar += (int.Parse(readerMateria[i].ToString()[3..])) - (int.Parse(readerMateria[i].ToString()[..2]));
                }
                else
                {
                    hrsxAsignar += 0;
                }
            }

            if (hrsAsignadas+hrsxAsignar <= hrpagadas)
            { 
                System.Diagnostics.Debug.WriteLine(hrpagadas+"+"+ (hrsAsignadas + hrsxAsignar));
                entra = true;
            }
           
            System.Diagnostics.Debug.WriteLine(entra);
            return entra;
        }
        public static int horasAsignadas(string profesor)
        {
            int[,] horas = llenaMatrizEySyH(profesor);
            int horasAsignadas = 0;
            for (int d = 0; d < horas.GetLength(0); d++)
            {
                horasAsignadas += horas[d, 2];
            }
            return horasAsignadas;
        }
        public static int getHrsAsignadas(string profesor)
        {
            SqlConnection connection = new SqlConnection(sCnn);
            SqlCommand cmd = new SqlCommand("select horasA from Maestros where nombre='"+profesor+"'", connection);
            connection.Open();
            SqlDataReader readerHorasA = cmd.ExecuteReader();
            readerHorasA.Read();
            int horasAprobadas = int.Parse(readerHorasA[0].ToString());
            readerHorasA.Close();
            return horasAprobadas;
        }

        private static string construyeQuery(params Object[] parametros)
        {
            string q = Querys[int.Parse(parametros[0].ToString())];
            for (int x = 1; x < parametros.Length; x++)
            {
                q = q.Replace("{" + x + "}", parametros[x].ToString());
            }
            return q;
        }

        private static void nonQuery(string q)
        {
            SqlConnection connection = new SqlConnection(sCnn);
            SqlCommand cmd = new SqlCommand(q, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }


        public static Boolean nochocanMaterias(string profesor, string materia, string grupo)
        {
            string[,] materiasExistentes = llenaMatrizdehoras(profesor);

            string newMateria = "select l, m, m1, j, v from Materias where materia='" + materia + "' and gpo='" + grupo + "'";
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmdG = new SqlCommand(newMateria, con);
            SqlDataReader readerNewMateria = cmdG.ExecuteReader();
            bool nochocan = true;
            string horaActualS = "", horaActualF = "", horaAsignarS = "", horaAsignarF = "";
            readerNewMateria.Read();

            for (int i = 0; i < materiasExistentes.GetLength(0); i++)
            {
                for (int j = 0; j < materiasExistentes.GetLength(1); j++)
                {
                    if (materiasExistentes[i, j] != "-")
                    {
                        horaActualS = materiasExistentes[i, j].Substring(0, 2);
                        horaActualF = materiasExistentes[i, j].Substring(3);
                    }
                    if (readerNewMateria[j].ToString() != "-")
                    {
                        horaAsignarS = readerNewMateria[j].ToString().Substring(0, 2);
                        horaAsignarF = readerNewMateria[j].ToString().Substring(3);
                    }
                    //System.Diagnostics.Debug.WriteLine(horaActualS+","+horaActualF+"\n"+
                    //horaAsignarS+","+horaAsignarF);
                    if (horaActualS == horaAsignarS || horaActualF == horaAsignarF)
                    {
                        nochocan = false;
                        //System.Diagnostics.Debug.WriteLine(chocan);
                        break;
                    }
                }
            }
            readerNewMateria.Close();
            con.Close();
            //System.Diagnostics.Debug.WriteLine(nochocan);
            return nochocan;
        }


        public static string[,] llenaMatrizdehoras(string profesor)
        {
            string consMateriasAsignadasya = "select l, m, m1, j, v from Horarios where profesor='" + profesor + "'";
            string filasString = "select count(1) from Horarios where profesor='" + profesor + "'";
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmdG = new SqlCommand(consMateriasAsignadasya, con);
            SqlDataReader materiasAsignadasya = cmdG.ExecuteReader();

            SqlCommand cmdF = new SqlCommand(filasString, con);
            SqlDataReader readerFilas = cmdF.ExecuteReader();
            readerFilas.Read();
            int filas = int.Parse(readerFilas[0].ToString());
            readerFilas.Close();

            string[,] horarios = new string[filas, 5];
            int filasCount = 0;

            while (materiasAsignadasya.Read())
            {
                for (int i = 0; i < 5; i++)
                {
                    horarios[filasCount, i] = materiasAsignadasya[i].ToString();
                }
                filasCount++;
            }
            materiasAsignadasya.Close();
            con.Close();
            return horarios;
        }
        public static int[,] llenaMatrizEySyH(string profesor)
        {
            string[,] horasExistentes = llenaMatrizdehoras(profesor);
            int[,] entradasSalidas = new int[horasExistentes.GetLength(1), 3];

            for (int d = 0; d < horasExistentes.GetLength(1); d++)
            {
                int menor = 20;
                for (int m = 0; m < horasExistentes.GetLength(0); m++)
                {
                    if (horasExistentes[m, d].ToString() != "-")
                    {
                        entradasSalidas[d, 2] += int.Parse(horasExistentes[m, d].ToString().Substring(3)) - int.Parse(horasExistentes[m, d].ToString().Substring(0, 2));
                        if (int.Parse(horasExistentes[m, d].ToString()[3..]) > entradasSalidas[d, 1])
                        {
                            entradasSalidas[d, 1] = int.Parse(horasExistentes[m, d].ToString().Substring(3));
                        }
                        if (int.Parse(horasExistentes[m, d].ToString().Substring(0, 2)) < menor)
                        {
                            menor = int.Parse(horasExistentes[m, d].ToString().Substring(0, 2));
                            entradasSalidas[d, 0] = menor;
                        }
                    }
                    else
                    {
                        entradasSalidas[d, 2] += 0;
                    }
                }
            }

            //for (int d = 0; d < entradasSalidas.GetLength(0); d++)
            //{
            //    for (int m = 0; m < entradasSalidas.GetLength(1); m++)
            //    {
            //        System.Diagnostics.Debug.WriteLine(entradasSalidas[d, m] + "///////");
            //    }
            //}
            //Instalar windows xp, y en la raiz instalar el paquete
            return entradasSalidas;
        }


        public static MateriasModel actualizarHorario(ListView vistaLista, string n)
        {
            List<MateriasModel> lista = new List<MateriasModel>();
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();
            SqlCommand cmd = new SqlCommand(construyeQuery(8, n), con);
            SqlDataReader r = cmd.ExecuteReader();

            MateriasModel model = new MateriasModel();

            if (r.HasRows)
            {
                while (r.Read())
                {
                    lista.Add(new MateriasModel
                    {
                        nombreMaterias = r["materia"].ToString(),
                        gpo = r["gpo"].ToString(),
                        l = r["l"].ToString(),
                        m = r["m"].ToString(),
                        m1 = r["m1"].ToString(),
                        j = r["j"].ToString(),
                        v = r["v"].ToString(),
                        profesor = r["profesor"].ToString(),
                    }
                    );
                }
                vistaLista.ItemsSource = lista;
            }
            r.Close();
            con.Close();
            return model;
        }

        public static string obtenerProfe()
        {
            SqlConnection con = new SqlConnection(sCnn);
            con.Open();

            SqlCommand cmdP = new SqlCommand(Querys[3], con);//obtención de profe aleatorio
            SqlDataReader rP = cmdP.ExecuteReader();
            rP.Read();
            string profesor = rP["nombre"].ToString();
            rP.Close();
            con.Close();
            return profesor;
        }



    }
}
