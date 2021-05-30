using System;
//using System.Collection;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using SudokuSolverRonen;


namespace SudokuSolverRonen
{
    public partial class SudokuSolverRonen : Form
    {
        // initialize
        public SudokuSolverRonen()
        {
            InitializeComponent();

            // initialize columns
            dataGridView1.AllowUserToAddRows = false; // don't add another row at the bottom
            dataGridView1.Columns.Add("C1", "S");
            dataGridView1.Columns.Add("C2", "u");
            dataGridView1.Columns.Add("C3", "D");
            dataGridView1.Columns.Add("C4", "o");
            dataGridView1.Columns.Add("C5", "K");
            dataGridView1.Columns.Add("C6", "u");
            dataGridView1.Columns.Add("C7", "");
            dataGridView1.Columns.Add("C8", "");
            dataGridView1.Columns.Add("C9", "");
            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Columns[i].Width = 40;
            }


            // initialize rows
            string[] row;
            //
            row = new string[] { "7", "", "", "5", "6", "", "", "", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "5", "", "1", "", "", "", "", "", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "", "", "", "", "", "", "2", "", "4" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "", "", "3", "", "", "", "", "7", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "", "", "", "", "3", "", "1", "", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "4", "", "6", "2", "1", "9", "", "", "" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "6", "", "5", "", "", "3", "", "", "1" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "", "", "", "8", "", "", "", "2", "6" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "", "8", "", "9", "", "", "", "", "" };
            dataGridView1.Rows.Add(row);
            //
            //row = new string[] { "", "", "", "2", "6", "", "7", "", "1" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "6", "8", "", "", "7", "", "", "9", "" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "1", "9", "", "", "", "4", "5", "", "" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "8", "2", "", "1", "", "", "", "4", "" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "", "", "4", "6", "", "2", "9", "", "" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "", "5", "", "", "", "3", "", "2", "8" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "", "", "9", "3", "", "", "", "7", "4" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "", "4", "", "", "5", "", "", "3", "6" };
            //dataGridView1.Rows.Add(row);
            //row = new string[] { "7", "", "3", "", "1", "8", "", "", "" };
            //dataGridView1.Rows.Add(row);


            // initialize borders
            //dataGridView1.Rows[3].Cells[3].Style.SelectionBackColor = Color.Red;
            //dataGridView1.Columns[5].DataGridView.BorderStyle = BorderStyle.Fixed3D;
            //dataGridView1.Rows[3].DataGridView.BorderStyle = BorderStyle.Fixed3D;
            //dataGridView1.Rows[0].Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");

            // initialize list of lists
            for (int i = 0; i < 81; i++)
            {
                OptionsPerCell.Add(new List<int> { });
            }

            // hash table for cell-number and cell indexes
            int cellNumber;
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    cellNumber = (i - 1) * 9 + j;
                    CellIndexHash.Add(cellNumber, i.ToString() + "," + j.ToString());
                }
            }

        }


        public void solveBtn_Click(object sender, EventArgs e)
        {
            List<int> rowData = new List<int>();
            List<int> colData = new List<int>();
            List<int> squareData;
            List<int> rowOptions;
            List<int> columnOptions;
            List<int> squareOptions;
            List<int> commonOptions;
            bool isSolved = false;
            int retry = 100;

            mainLabel.Text = "Solving...";
            mainLabel.Refresh();

            for (int iterations = 0; iterations <= retry; iterations++) // iterations
            {
                for (rowNum = 1; rowNum <= 9; rowNum++)
                {
                    for (colNum = 1; colNum <= 9; colNum++)
                    {
                        // solve for cell
                        cellValue = dataGridView1.Rows[rowNum - 1].Cells[colNum - 1].Value.ToString();
                        if (cellValue == "")
                        {
                            // get row data
                            for (int i = 1; i <= 9; i++)
                            {
                                cellValue = dataGridView1.Rows[rowNum - 1].Cells[i - 1].Value.ToString();
                                if (cellValue != "")
                                {
                                    rowData.Add(Convert.ToInt32(cellValue));
                                }
                            }

                            // get column data
                            for (int j = 1; j <= 9; j++)
                            {
                                cellValue = dataGridView1.Rows[j - 1].Cells[colNum - 1].Value.ToString();
                                if (cellValue != "")
                                {
                                    colData.Add(Convert.ToInt32(cellValue));
                                }
                            }

                            // get square data
                            sqrPos = getSqrPos(rowNum, colNum);
                            squareData = getSqrData(sqrPos);

                            // try to solve by existing data
                            rowOptions = getRowOptions(rowData);
                            columnOptions = getColumnOptions(colData);
                            squareOptions = getSquareOptions(squareData);
                            commonOptions = getCommonResults(rowOptions, columnOptions, squareOptions);
                            if (commonOptions.Count == 1)
                            {
                                dataGridView1.Rows[rowNum - 1].Cells[colNum - 1].Value = commonOptions[0].ToString();
                                dataGridView1.Refresh();
                                isSolved = checkIfSolved();
                                if (isSolved)
                                {
                                    mainLabel.Text = "Solved!";
                                    break;
                                }
                            }
                            else
                            {
                                cellNumber = getCellNumber(rowNum, colNum);
                                OptionsPerCell[cellNumber - 1].Clear();
                                for (int p = 0; p < commonOptions.Count; p++)
                                {
                                    OptionsPerCell[cellNumber - 1].Add(commonOptions[p]);
                                }
                            }
                        }

                        rowData = new List<int>();
                        colData = new List<int>();
                        squareData = new List<int>();
                    }
                    if (isSolved)
                    {
                        break;
                    }
                }

                // try to reduce OptionsPerCell
                reduceOptions();
                isSolved = checkIfSolved();
                if (isSolved)
                {
                    mainLabel.Text = "Solved!";
                    break;
                }

                // try solving by guessing
                if (iterations > retry / 2)
                {
                    guessNumber();
                    isSolved = checkIfSolved();
                    if (isSolved)
                    {
                        mainLabel.Text = "Solved!";
                        break;
                    }
                }
                if (iterations == retry && !isSolved) // last iteration, not solved
                {
                    mainLabel.Text = "Failed to solve";
                }
            }
        }

        public void guessNumber()
        {

        }
        

        public void reduceOptions()
        {
            int cellIndex = -1;

            // for square 1: cells 1 ,2 ,3 ,10,11,12,19,20,21
            reduceSquareOptions(cellIndex, 1, 2, 3, 10, 11, 12, 19, 20, 21);

            // for square 2: cells 4 ,5 ,6 ,13,14,15,22,23,24
            reduceSquareOptions(cellIndex, 4, 5, 6, 13, 14, 15, 22, 23, 24);

            // for square 3: cells 7 ,8 ,9 ,16,17,18,25,26,27
            reduceSquareOptions(cellIndex, 7, 8, 9, 16, 17, 18, 25, 26, 27);

            // for square 4: cells 28,29,30,37,38,39,46,47,48
            reduceSquareOptions(cellIndex, 28, 29, 30, 37, 38, 39, 46, 47, 48);

            // for square 5: cells 31,32,33,40,41,42,49,50,51
            reduceSquareOptions(cellIndex, 31, 32, 33, 40, 41, 42, 49, 50, 51);

            // for square 6: cells 34,35,36,43,44,45,52,53,54
            reduceSquareOptions(cellIndex, 34, 35, 36, 43, 44, 45, 52, 53, 54);

            // for square 7: cells 55,56,57,64,65,66,73,74,75
            reduceSquareOptions(cellIndex, 55, 56, 57, 64, 65, 66, 73, 74, 75);

            // for square 8: cells 58,59,60,67,68,69,76,77,78
            reduceSquareOptions(cellIndex, 58, 59, 60, 67, 68, 69, 76, 77, 78);

            // for square 9: cells 61,62,63,70,71,72,79,80,81
            reduceSquareOptions(cellIndex, 61, 62, 63, 70, 71, 72, 79, 80, 81);

        }

        public void reduceSquareOptions(int cellIndex, int cell1, int cell2, int cell3, int cell4, int cell5, int cell6, int cell7, int cell8, int cell9)
        {
            List<int> sqrOptions = new List<int>();
            sqrOptions.Clear();
            string indexArr;
            string[] indexes;

            // switch to cell index
            cell1 -= 1;
            cell2 -= 1;
            cell3 -= 1;
            cell4 -= 1;
            cell5 -= 1;
            cell6 -= 1;
            cell7 -= 1;
            cell8 -= 1;
            cell9 -= 1;
            cellIndex = -1;

            for (int i = 0; i < OptionsPerCell[cell1].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell1][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell2].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell2][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell3].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell3][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell4].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell4][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell5].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell5][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell6].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell6][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell7].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell7][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell8].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell8][i]);
            }
            for (int i = 0; i < OptionsPerCell[cell9].Count; i++)
            {
                sqrOptions.Add(OptionsPerCell[cell9][i]);
            }

            // remove duplicate results
            var res = sqrOptions.GroupBy(x => x).Where(g => g.Count() == 1).Select(g => g.Key);
            sqrOptions = res.ToList();

            //
            if (sqrOptions.Count == 1)
            {
                if (OptionsPerCell[cell1].Contains(sqrOptions[0]))
                {
                    cellIndex = cell1;
                }
                else if (OptionsPerCell[cell2].Contains(sqrOptions[0]))
                {
                    cellIndex = cell2;
                }
                else if (OptionsPerCell[cell3].Contains(sqrOptions[0]))
                {
                    cellIndex = cell3;
                }
                else if (OptionsPerCell[cell4].Contains(sqrOptions[0]))
                {
                    cellIndex = cell4;
                }
                else if (OptionsPerCell[cell5].Contains(sqrOptions[0]))
                {
                    cellIndex = cell5;
                }
                else if (OptionsPerCell[cell6].Contains(sqrOptions[0]))
                {
                    cellIndex = cell6;
                }
                else if (OptionsPerCell[cell7].Contains(sqrOptions[0]))
                {
                    cellIndex = cell7;
                }
                else if (OptionsPerCell[cell8].Contains(sqrOptions[0]))
                {
                    cellIndex = cell8;
                }
                else if (OptionsPerCell[cell9].Contains(sqrOptions[0]))
                {
                    cellIndex = cell9;
                }

                // update options list
                OptionsPerCell[cellIndex].Clear();
                // update table
                cellIndex++; // switch to cell number
                indexArr = (string)CellIndexHash[cellIndex];
                indexes = SplitStringBySeperator(indexArr, ",");
                rowNum = Convert.ToInt32(indexes[0]);
                colNum = Convert.ToInt32(indexes[1]);
                dataGridView1.Rows[rowNum - 1].Cells[colNum - 1].Value = sqrOptions[0].ToString();
                dataGridView1.Refresh();

            }
        }

        public int getCellNumber(int row, int column)
        {
            int cellPos = 1;
            //
            cellPos *= (row - 1) * 9;
            cellPos += column;
            //
            return cellPos;
        }

        public bool checkIfSolved()
        {
            string temp;
            int counter = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    temp = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    if (temp != "")
                    {
                        counter++;
                        if (counter == 81)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public List<int> getSqrData(int sqrPos)
        {
            List<int> retValues = new List<int>();

            switch (sqrPos)
            {
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 2:
                    for (int i = 3; i < 6; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 3:
                    for (int i = 6; i < 9; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 4:
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 3; j < 6; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 5:
                    for (int i = 3; i < 6; i++)
                    {
                        for (int j = 3; j < 6; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 6:
                    for (int i = 6; i < 9; i++)
                    {
                        for (int j = 3; j < 6; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 7:
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 6; j < 9; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 8:
                    for (int i = 3; i < 6; i++)
                    {
                        for (int j = 6; j < 9; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                case 9:
                    for (int i = 6; i < 9; i++)
                    {
                        for (int j = 6; j < 9; j++)
                        {
                            cellValue = dataGridView1.Rows[j].Cells[i].Value.ToString();
                            if (cellValue != "")
                            {
                                retValues.Add(Convert.ToInt32(cellValue));
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            return retValues;
        }

        public List<int> getRowOptions(List<int> rowData)
        {
            List<int> retValues = new List<int>();

            for (int j = 1; j <= 9; j++)
            {
                alreadyExists = false;

                for (int i = 0; i < rowData.Count; i++)
                {
                    if (j == rowData[i])
                    {
                        alreadyExists = true;
                    }
                }
                if (!alreadyExists)
                {
                    retValues.Add(j);
                }
            }
            return retValues;
        }

        public List<int> getColumnOptions(List<int> colData)
        {
            List<int> retValues = new List<int>();

            for (int j = 1; j <= 9; j++)
            {
                alreadyExists = false;

                for (int i = 0; i < colData.Count; i++)
                {
                    if (j == colData[i])
                    {
                        alreadyExists = true;
                    }
                }
                if (!alreadyExists)
                {
                    retValues.Add(j);
                }
            }
            return retValues;
        }

        public List<int> getSquareOptions(List<int> squareData)
        {
            List<int> retValues = new List<int>();

            for (int j = 1; j <= 9; j++)
            {
                alreadyExists = false;

                for (int i = 0; i < squareData.Count; i++)
                {
                    if (j == squareData[i])
                    {
                        alreadyExists = true;
                    }
                }
                if (!alreadyExists)
                {
                    retValues.Add(j);
                }
            }
            return retValues;
        }

        public List<int> getCommonResults(List<int> rowOptions, List<int> columnOptions, List<int> squareOptions)
        {
            List<int> retValues = new List<int>();
            List<int> tempList = new List<int>();

            // compare rows and columns
            for (int i = 0; i < rowOptions.Count; i++)
            {
                for (int j = 0; j < columnOptions.Count; j++)
                {
                    if (rowOptions[i] == columnOptions[j])
                    {
                        tempList.Add(rowOptions[i]);
                    }
                }
            }

            // compare result and square
            for (int i = 0; i < tempList.Count; i++)
            {
                for (int j = 0; j < squareOptions.Count; j++)
                {
                    if (tempList[i] == squareOptions[j])
                    {
                        retValues.Add(tempList[i]);
                    }
                }
            }

            // 
            return retValues;
        }

        public int getSqrPos(int rowNum, int colNum)
        {
            // sqr 1, sqr 2, sqr 3
            // sqr 4, sqr 5, sqr 6
            // sqr 7, sqr 8, sqr 9

            if (rowNum > 0 && rowNum <= 3)
            {
                if (colNum > 0 && colNum <= 3)
                {
                    sqrPos = 1;
                }
                if (colNum > 3 && colNum <= 6)
                {
                    sqrPos = 2;
                }
                if (colNum > 6 && colNum <= 9)
                {
                    sqrPos = 3;
                }

            }
            if (rowNum > 3 && rowNum <= 6)
            {
                if (colNum > 0 && colNum <= 3)
                {
                    sqrPos = 4;
                }
                if (colNum > 3 && colNum <= 6)
                {
                    sqrPos = 5;
                }
                if (colNum > 6 && colNum <= 9)
                {
                    sqrPos = 6;
                }
            }
            if (rowNum > 6 && rowNum <= 9)
            {
                if (colNum > 0 && colNum <= 3)
                {
                    sqrPos = 7;
                }
                if (colNum > 3 && colNum <= 6)
                {
                    sqrPos = 8;
                }
                if (colNum > 6 && colNum <= 9)
                {
                    sqrPos = 9;
                }
            }

            return sqrPos;
        }

        protected string[] SplitStringBySeperator(string Str, string Seperator)
        {
            string[] Seperators = new string[2];
            Seperators[0] = Seperator;
            Seperators[1] = "";
            string[] sArray = Str.Split(Seperators, StringSplitOptions.None);

            return sArray;
        }






        #region Variables

        string cellValue;
        int rowNum;
        int colNum;
        int sqrPos;
        int cellNumber;
        bool alreadyExists;

        List<List<int>> OptionsPerCell = new List<List<int>>();

        Hashtable CellIndexHash = new Hashtable();

        #endregion Variables

    }
}
