using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MesajTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string numara;

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-K0C08G8;Initial Catalog=Message;Integrated Security=True");


        void gelenKutusu()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("Select MESAJID as 'Mesaj Kodu',(ad+' '+soyad) as GONDEREN,BASLIK,ICERIK From TBLMESAJLAR inner join TBLKISILER on TBLMESAJLAR.GONDEREN=TBLKISILER.NUMARA where ALICI=" + numara, baglanti);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
        }

        void gidenKutusu()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select MESAJID as 'Mesaj Kodu',(ad+' '+soyad) as ALICI,BASLIK,ICERIK From TBLMESAJLAR inner join TBLKISILER on TBLMESAJLAR.ALICI=TBLKISILER.NUMARA where GONDEREN=" + numara, baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LblNumara.Text = numara;

            gelenKutusu();
            gidenKutusu();

            // Ad Soyad Çekme
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select Ad,Soyad From TBLKISILER where numara=" + numara, baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Insert into TBLMESAJLAR (GONDEREN,ALICI,BASLIK,ICERIK) values (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", numara);
            komut.Parameters.AddWithValue("@p2", maskedTextBox1.Text);
            komut.Parameters.AddWithValue("@p3", textBox1.Text);
            komut.Parameters.AddWithValue("@p4", richTextBox1.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Mesajınız İletildi");
            maskedTextBox1.Text = "";
            textBox1.Text = "";
            richTextBox1.Text = "";
            gidenKutusu();
        }
    }
}
