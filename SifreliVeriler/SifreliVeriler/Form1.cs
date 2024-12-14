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

namespace SifreliVeriler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void listele()
        {
            // SQL'den tüm verileri çeker
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tbl_Veriler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Yeni tablo oluştur (çözülmüş veriler için)
            DataTable cozumluDt = new DataTable();

            // Orijinal sütunları tabloya ekler
            cozumluDt.Columns.Add("AD", typeof(string));
            cozumluDt.Columns.Add("SOYAD", typeof(string));
            cozumluDt.Columns.Add("MAIL", typeof(string));
            cozumluDt.Columns.Add("SIFRE", typeof(string));
            cozumluDt.Columns.Add("HESAPNO", typeof(string));

            // Her bir satırı işler ve şifre çözme işlemini yapar
            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = cozumluDt.NewRow();

                // Şifrelenmiş veriyi çözüyor
                newRow["AD"] = SifreCoz(row["AD"].ToString());
                newRow["SOYAD"] = SifreCoz(row["SOYAD"].ToString());
                newRow["MAIL"] = SifreCoz(row["MAIL"].ToString());
                newRow["SIFRE"] = SifreCoz(row["SIFRE"].ToString());
                newRow["HESAPNO"] = SifreCoz(row["HESAPNO"].ToString());

                // Yeni tabloya ekler
                cozumluDt.Rows.Add(newRow);
            }

            // Çözülmüş tabloyu DataGridView'e bağlar
            dataGridView1.DataSource = cozumluDt;
        }

        // Şifre çözme işlemi
        string SifreCoz(string sifreliVeri)
        {
            try
            {
                byte[] cozumDizi = Convert.FromBase64String(sifreliVeri); // Base64 çözümleme
                return ASCIIEncoding.ASCII.GetString(cozumDizi); // Byte[]'i string'e çevirir
            }
            catch
            {
                return "Çözüm Hatası"; // Hatalı çözümleme durumunda
            }
        }






        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=EMIR\SQLEXPRESS;Initial Catalog=DbSifreleme;Integrated Security=True;Encrypt=False");

        private void button2_Click(object sender, EventArgs e)
        {
            string metin=txtAd.Text;

            //byte türünde dizi oluşturup her bir karakteri parçaladım.
            byte[] adDizi=ASCIIEncoding.ASCII.GetBytes(metin);

            //şifreli veriye toBase formatında her bir karakteri şifreleme işlemi
            string adSifre=Convert.ToBase64String(adDizi);

            string soyad=txtSoyad.Text;
            byte[] soyadDizi = ASCIIEncoding.ASCII.GetBytes(soyad);
            string soyadSifre = Convert.ToBase64String(soyadDizi);

            string mail=txtMail.Text;
            byte[] mailDizi = ASCIIEncoding.ASCII.GetBytes(mail);
            string mailSifre = Convert.ToBase64String(mailDizi);

            string sifre = txtSifre.Text;
            byte[] sifreDizi = ASCIIEncoding.ASCII.GetBytes(sifre);
            string sifreSifre = Convert.ToBase64String(sifreDizi);

            string hesapNo = txtHesapNoo.Text;
            byte[] hesapDizi = ASCIIEncoding.ASCII.GetBytes(hesapNo);
            string hesapSifre = Convert.ToBase64String(hesapDizi);

            baglanti.Open();

            SqlCommand komut = new SqlCommand("INSERT INTO Tbl_Veriler (AD,SOYAD,MAIL,SIFRE,HESAPNO)" +
                " VALUES (@p1,@p2,@p3,@p4,@p5)",baglanti);
            komut.Parameters.AddWithValue("@p1",adSifre);
            komut.Parameters.AddWithValue("@p2", soyadSifre);
            komut.Parameters.AddWithValue("@p3", mailSifre);
            komut.Parameters.AddWithValue("@p4", sifreSifre);
            komut.Parameters.AddWithValue("@p5", hesapSifre);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Veriler eklendi!");

            baglanti.Close();



        }

        private void button3_Click(object sender, EventArgs e)
        {
            string adCozum = txtAd.Text;
            byte[] adCozumDizi=Convert.FromBase64String(adCozum);
            string adVerisi=ASCIIEncoding.ASCII.GetString(adCozumDizi);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }
    }
}
