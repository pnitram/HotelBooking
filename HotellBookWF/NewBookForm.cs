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
    public partial class NewBookForm : Form
    {

        private BindingList<string> ResList { get; set; }
        private string _bookingString;
        private DataSet Ds { get; }
        private MySqlDataAdapter Da { get; }
        private List<string> _tempList = new List<string>();



        public NewBookForm(BindingList<string> resList, DataSet ds, MySqlDataAdapter da)
        {
            ResList = resList;
            Ds = ds;
            Da = da;

            InitializeComponent();
            Size = new Size(750, 400);
        }
        public NewBookForm()
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show(@"Du må taste inn både fornavn og etternavn!");
                return;
            }

            else if (monthCalendar1.SelectionStart.Date < DateTime.Now.Date)
            {
                MessageBox.Show(@"Fra dato kan ikke være før dagens dato!");
                return;
            }

            else if (monthCalendar2.SelectionStart.Date < monthCalendar1.SelectionStart.Date)
            {
                MessageBox.Show(@"Du kan ikke velge en til dato som er tidligere enn fra dato!");
                return;
            }

            _bookingString = textBox1.Text + " " + textBox2.Text + " \n" + monthCalendar1.SelectionStart.Year + "-" +
                             monthCalendar1.SelectionStart.Month + "-" + monthCalendar1.SelectionStart.Day + " --> " +
                             monthCalendar2.SelectionStart.Year + "-" + monthCalendar2.SelectionStart.Month + "-" +
                             monthCalendar2.SelectionStart.Day;
            

            DataRow nrad;

            nrad = Ds.Tables["booking"].NewRow();
            nrad["fornavn"] = textBox1.Text;
            nrad["etternavn"] = textBox2.Text;
            nrad["datoFra"] = monthCalendar1.SelectionStart.Year + "-" + monthCalendar1.SelectionStart.Month + "-" +
                              monthCalendar1.SelectionStart.Day;
            nrad["datoTil"] = monthCalendar2.SelectionStart.Year + "-" + monthCalendar2.SelectionStart.Month + "-" +
                              monthCalendar2.SelectionStart.Day;

            Ds.Tables["booking"].Rows.Add(nrad);
            Da.Update(Ds, "booking");

            //ResList.Add(_bookingString);

            Dispose();
        }

    }
}