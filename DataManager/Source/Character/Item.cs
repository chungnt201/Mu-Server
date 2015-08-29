﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zDBManager.Character
{
    public partial class Item : Form
    {
        public Item()
        {
            InitializeComponent();

            charPanel1.Parent = pictureBox1;
            equipPanel1.Parent = pictureBox1;
            equipPanel2.Parent = pictureBox1;
            equipPanel3.Parent = pictureBox1;
            equipPanel4.Parent = pictureBox1;
            equipPanel5.Parent = pictureBox1;
            equipPanel6.Parent = pictureBox1;
        }

        public string CharName;

        protected byte[] loadInventory(string charName)
        {
            if (charName == "")
            {
                return null;
            }

            byte[] inventory = null;

            DBLite.dbMu.Read("select Inventory, Money from character where Name = '" + charName + "'");
            DBLite.dbMu.Fetch();
            inventory = DBLite.dbMu.GetAsBinary("Inventory");
            textBox1.Text = DBLite.dbMu.GetAsInteger("Money").ToString();
            DBLite.dbMu.Close();

            return inventory;
        }

        private void Item_Load(object sender, EventArgs e)
        {
            Text = CharName + " Inventory";
            charPanel1.Editor = equipEditor1;
            equipPanel1.setSize(8, 8);
            equipPanel1.Editor = equipEditor1;
            equipPanel2.setSize(8, 4);
            equipPanel2.Editor = equipEditor1;
            equipPanel3.setSize(8, 4);
            equipPanel3.Editor = equipEditor1;
            equipPanel4.setSize(8, 4);
            equipPanel4.Editor = equipEditor1;
            equipPanel5.setSize(8, 4);
            equipPanel5.Editor = equipEditor1;
            equipPanel6.setSize(8, 4);
            equipPanel6.Editor = equipEditor1;

            equipEditor1.LoadData();

            byte[] inventory = loadInventory(CharName);
            if (inventory != null)
            {
                charPanel1.clearData();
                if (!charPanel1.loadItemsByCodes(inventory, 0, CharPanel.EquipNum * EquipItem.ITEM_SIZE))
                {
                    MessageBox.Show(string.Format("[{0}] Error loading body equipment", CharName));
                }

                equipPanel1.clearData();
                if (!equipPanel1.loadItemsByCodes(inventory, CharPanel.EquipNum * EquipItem.ITEM_SIZE, inventory.Length - CharPanel.EquipNum * EquipItem.ITEM_SIZE - equipPanel2.XNum * equipPanel2.YNum * EquipItem.ITEM_SIZE))
                {
                    MessageBox.Show(string.Format("[{0}] Error loading inventory equipment", CharName));
                }

                Int32 Offset = CharPanel.EquipNum * EquipItem.ITEM_SIZE + equipPanel1.XNum * equipPanel1.YNum * EquipItem.ITEM_SIZE;

                equipPanel2.clearData();
                if (!equipPanel2.loadItemsByCodes(inventory, Offset, inventory.Length))
                {
                    MessageBox.Show(string.Format("[{0}] Error loading pshop equipment", CharName));
                }

                Offset += equipPanel2.XNum * equipPanel2.YNum * EquipItem.ITEM_SIZE;

                equipPanel3.clearData();
                if (!equipPanel3.loadItemsByCodes(inventory, Offset, inventory.Length))
                {
                    MessageBox.Show(string.Format("[{0}] Error loading pshop equipment", CharName));
                }

                Offset += equipPanel3.XNum * equipPanel3.YNum * EquipItem.ITEM_SIZE;

                equipPanel4.clearData();
                if (!equipPanel4.loadItemsByCodes(inventory, Offset, inventory.Length))
                {
                    MessageBox.Show(string.Format("[{0}] Error loading pshop equipment", CharName));
                }

                Offset += equipPanel4.XNum * equipPanel4.YNum * EquipItem.ITEM_SIZE;

                equipPanel5.clearData();
                if (!equipPanel5.loadItemsByCodes(inventory, Offset, inventory.Length))
                {
                    MessageBox.Show(string.Format("[{0}] Error loading pshop equipment", CharName));
                }

                Offset += equipPanel5.XNum * equipPanel5.YNum * EquipItem.ITEM_SIZE;

                equipPanel6.clearData();
                if (!equipPanel6.loadItemsByCodes(inventory, Offset, inventory.Length))
                {
                    MessageBox.Show(string.Format("[{0}] Error loading pshop equipment", CharName));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = string.Format("update character set Inventory=0x{0}{1}{2}{3}{4}{5}{6} where Name='{7}'",
                charPanel1.getEquipCodes(), equipPanel1.getEquipCodes(), equipPanel2.getEquipCodes(), 
                equipPanel3.getEquipCodes(), equipPanel4.getEquipCodes(), equipPanel5.getEquipCodes(),
                equipPanel6.getEquipCodes(), CharName);
            DBLite.dbMu.Exec(sql);
            DBLite.dbMu.Close();
            MessageBox.Show("Inventory Edited", "zDBManager", MessageBoxButtons.OK);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
