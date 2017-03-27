using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HotellBookWF
{
    public partial class HotelBookForm : Form
    {
        private MySqlCommandBuilder _cb;
        private MySqlCommand _cmd;
        private MySqlDataAdapter _da;
        private MySqlConnection _dbconn;
        private DataSet _ds;
        private DataSet _dsRomRes;
        private string _myconnectionstring;

        private List<string> _tempList = new List<string>();
        public BindingList<string> ResList;

        public HotelBookForm()
        {
            InitializeComponent();
            Size = new Size(1100, 700);
        }

        private void HotellBookForm_Load(object sender, EventArgs e)
        {
            UpdateBookinListDb();
            AddRooms(tabPage1, 1, _dsRomRes);
            AddRooms(tabPage2, 2, _dsRomRes);
            AddRooms(tabPage3, 3, _dsRomRes);
        }

        public void UpdateBookinListDb()
        {
            _myconnectionstring = "Database=Test; Data Source = localhost; User = test; Password = test";
            _dbconn = new MySqlConnection(_myconnectionstring);
            var sql1 = "SELECT * FROM booking";
            _cmd = new MySqlCommand(sql1, _dbconn);
            _da = new MySqlDataAdapter(_cmd);
            _cb = new MySqlCommandBuilder(_da);
            _dsRomRes = new DataSet("bookinger");
            _da.Fill(_dsRomRes, "booking");
            //Nå kan vi skrive ut datasettet som .xml
            _dsRomRes.WriteXml("test.xml");

            var sql2 = "SELECT * FROM booking WHERE romNr IS NULL";
            _cmd = new MySqlCommand(sql2, _dbconn);
            _da = new MySqlDataAdapter(_cmd);
            _cb = new MySqlCommandBuilder(_da);
            _ds = new DataSet("bookinger");
            _da.Fill(_ds, "booking");

            _tempList = (from r in _ds.Tables["booking"].AsEnumerable()
                select
                r.Field<int>("bookingId") + " " + r.Field<string>("fornavn") + " " + r.Field<string>("etternavn") + ": " +
                r.Field<DateTime>("datoFra").Year + "-" + r.Field<DateTime>("datoFra").Month + "-" +
                r.Field<DateTime>("datoFra").Day + " > " + r.Field<DateTime>("datoTil").Year + "-" +
                r.Field<DateTime>("datoTil").Month + "-" + r.Field<DateTime>("datoTil").Day).ToList();

            ResList = new BindingList<string>(_tempList);
            listBox1.DataSource = ResList;
        }


        private void AddRooms(TabPage tab, int floor, DataSet ds)
        {
            int i, j;
            var count = 1;
            // Adds rooms. 12 rooms pr. floor
            for (i = 1; i <= 3; i++)
            for (j = 1; j <= 4; j++)
            {
                var p = new Panel();
                var l = new Label();

                p.Location = new Point(i * 125, j * 75);
                p.BorderStyle = BorderStyle.FixedSingle;
                p.Width = 120;
                p.Height = 70;
                l.Location = new Point(10, 10);
                l.Width = 120;
                l.Height = 70;
                if (count < 10)
                {
                    p.Name = $"{floor}0{count}";
                    l.Text = $@"{floor}0{count}";
                }
                else if (count >= 10 && count < 20)
                {
                    p.Name = $"{floor}{count}";
                    l.Text = $@"{floor}{count}";
                }

                p.BackColor = Color.GreenYellow;
                p.AllowDrop = true;

                // Setter opp hendelseshåndterere for DragOver og DragDrop
                p.DragDrop += HotellBookForm_DragDrop;
                p.DragOver += HotellBookForm_DragOver;
                //p.Visible = true;


                //Kobler Click-hendelsen til en felles event-handler
                p.Click += doit_click;
                //Viser at flere event-handlere kan betjene samme 
                if (i < 5) p.Click += doit2_click;
                tab.Controls.Add(p);
                p.Controls.Add(l);


                //ResList for å sjekke om farge kan endres. Her må romtilstand lagres


                foreach (DataRow rad in ds.Tables["booking"].Rows)
                    try
                    {
                            if ((DateTime) rad["datoFra"] >= DateTime.Now.Date && (int) rad["romNr"] == int.Parse(p.Name))
                        {
                            if (!((DateTime) rad["datoTil"] < DateTime.Now.Date) && (!((DateTime) rad["datoFra"] > DateTime.Now.Date)))
                            {
                                    MessageBox.Show("Rom funnet " + p.Name);
                                p.BackColor = Color.Red;
                                p.AllowDrop = false;
                                l.Text = p.Name + $" - " + "Id: " + rad["bookingId"] + " " + rad["fornavn"] + " " +
                                         rad["etternavn"] + " " + rad["datoFra"].ToString().Substring(0, 10) + " > " +
                                         rad["datoTil"].ToString().Substring(0, 10);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }


                //MessageBox.Show(_dsRomRes.Tables["booking"].Select("*").ToString());

                /*
                if (_dsRomRes.Tables["booking"].Rows[0]["romNr"] == p.Name)
                {
                    p.BackColor = Color.Aqua;
                }
                */
                count++;
            }
        }

        private void doit_click(object sender, EventArgs e)
        {
            MessageBox.Show("Typen er: " + sender.GetType().FullName);
            //Typekonverter for å få tak i egenskapene til Control


            var c = (Control) sender;


            MessageBox.Show("Sender er " + c.Name);
        }

        private void doit2_click(object sender, EventArgs e)
        {
            var c = (Control) sender;
            MessageBox.Show("doit2: Sender er " + c.Name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Form newbook = new NewBookForm(ResList, _ds, _da))
            {
                newbook.FormClosed += HotellBookForm_FormClosed;
                newbook.ShowDialog();
            }
        }

        private void HotellBookForm_DragDrop(object sender, DragEventArgs e)
        {
            var c = (Control) sender;
            // item vil inneholde navnet som dras fra nedtrekkslisten
            if (c.BackColor == Color.Red)
            {
                AllowDrop = false;
            }

            else
            {
                var item = e.Data.GetData(typeof(string));
                var flooRoomNrLabel = c.Name;

                MessageBox.Show(item.ToString());

                char[] separatingChars = {':', '>', ' '};
                var text = item.ToString();
                var values = text.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);

                //var sql = "SELECT * FROM booking WHERE bookingId =" + values[0];
                var sql ="SELECT * FROM booking WHERE (datoFra BETWEEN CAST('" + values[3] + "' AS DATE) AND CAST('" + values[4] + "' AS DATE) AND datotil BETWEEN CAST('" + values[3] + "' AS DATE) AND CAST('" + values[4] +"' AS DATE) AND romNr =" + c.Name +")";
                using (MySqlCommand cmd = new MySqlCommand(sql, _dbconn))
                {
                    _dbconn.Open();
                    var numRowsUpdated = cmd.ExecuteScalar();

                    if (numRowsUpdated == null)
                    {
                        c.Controls[0].Text = $"{flooRoomNrLabel}\n{item}";
                        c.BackColor = Color.Red;
                        var sql2 = "SELECT * FROM booking WHERE bookingId =" + values[0];
                        _da = new MySqlDataAdapter(sql2, _dbconn);
                        _cb = new MySqlCommandBuilder(_da);
                        _ds.Tables["booking"].Rows[0]["romNr"] = c.Name;
                        _da.Update(_ds, "booking");
                        ResList.Remove((string)item);

                    }
                    else
                    {
                        MessageBox.Show(@"Dette rommet er desverre resevert i løp av perioden " +values[3] + @" og " + values[4] + @". Venligst velg et annet rom.");
                        this.Refresh();
                        //                        FormClosed += (o, a) => new HotelBookForm().ShowDialog();
                        //                        Hide();
                        //                        Close();
                    }
                    _dbconn.Close();

                }




                /*
                var sql = "UPDATE booking SET romNr=" + c.Name + " WHERE bookingId=" + values[0];
                try
                {
                    using (MySqlConnection cn = new MySqlConnection(_myconnectionstring))
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = cn;
                        cmd.CommandText = "UPDATE booking SET romNr=" + c.Name + " WHERE bookingId=" + values[0];
                        cn.Open();
                        int numRowsUpdated = cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        MessageBox.Show(@"Rows affected: " + numRowsUpdated);
                        UpdateBookinListDb();
                    }
                }
                
                catch
                {
                    MessageBox.Show(@"Error: DB not updated!");
                }
                */

                //                 DEBUG
                //                foreach (var v in values)
                //                    MessageBox.Show(v);

                
            }
        }

        private void HotellBookForm_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                listBox1.DoDragDrop(listBox1.SelectedItem, DragDropEffects.All);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                try
                {
                    listBox1.DoDragDrop(listBox1.SelectedItem, DragDropEffects.Move);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateBookinListDb();
        }

        private void HotellBookForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateBookinListDb();
        }
    }
}