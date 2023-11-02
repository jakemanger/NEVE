using UnityEngine;
using System.Collections;

public class PathRandomization : MonoBehaviour {
    public int number_of_columns;
    public int number_of_rows;
    public float Stim_Size;
    public int screenYpixels;
    public int screenXpixels;
    public int seed;

    private string[] Directions_x;
    private string[] Directions_y;
    public int[] Order;
    public float[] XPos;
    public float[] YPos;

    public void Initialise() {
        // Initialize random number generator
        Random.InitState(seed);

        // Generate random starting direction for x and y
        int x = Random.Range(1, 3);
        int y = Random.Range(1, 3);
        string D_x1, D_x2, D_y1, D_y2;

        if (x == 1) {
            D_x1 = "LR";
            D_x2 = "RL";
        } else {
            D_x1 = "RL";
            D_x2 = "LR";
        }

        if (y == 1) {
            D_y1 = "UD";
            D_y2 = "DU";
        } else {
            D_y1 = "DU";
            D_y2 = "UD";
        }

        // Generate random starting position
        int S_Column = Random.Range(1, number_of_columns + 1);
        int S_Row = Random.Range(1, number_of_rows + 1);

        // Create direction arrays
        Directions_y = new string[number_of_columns];
        for (int i = 0; i < number_of_columns; i++) {
            if (S_Column % 2 == 1) {
                Directions_y[i] = (i % 2 == 0) ? D_y1 : D_y2;
            } else {
                Directions_y[i] = (i % 2 == 0) ? D_y2 : D_y1;
            }
        }

        Directions_x = new string[number_of_rows];
        for (int i = 0; i < number_of_rows; i++) {
            if (S_Row % 2 == 1) {
                Directions_x[i] = (i % 2 == 0) ? D_x1 : D_x2;
            } else {
                Directions_x[i] = (i % 2 == 0) ? D_x2 : D_x1;
            }
        }

        // Create randomized order of scanning the screen
        Order = new int[number_of_rows + number_of_columns];
        ArrayList possibilities = new ArrayList();
        for (int i = 1; i <= number_of_rows + number_of_columns; i++) {
            possibilities.Add(i);
        }

        int l = 0;
        while (possibilities.Count > 0) {
            int p = Random.Range(0, possibilities.Count);
            Order[l] = (int)possibilities[p];
            possibilities.RemoveAt(p);
            l++;
        }

        // Create arrays for x and y positions
        XPos = new float[Order.Length];
        YPos = new float[Order.Length];

        // Update x and y positions based on order and directions
        for (int m = 0; m < Order.Length; m++) {
            string D;
            if (Order[m] <= number_of_rows) {
                D = Directions_x[Order[m] - 1];
            } else {
                D = Directions_y[Order[m] - number_of_rows - 1];
            }

            switch (D) {
                case "LR":
                    // XPos[m] = -Stim_Size / 2;
                    // YPos[m] = ((Order[m] - 0.5f) * Stim_Size);
                    XPos[m] = 0;
                    YPos[m] = (Order[m] * Stim_Size);
                    break;
                case "RL":
                    XPos[m] = screenXpixels;
                    YPos[m] = (Order[m] * Stim_Size);
                    break;
                case "UD":
                    XPos[m] = ((Order[m] - number_of_rows) * Stim_Size);
                    YPos[m] = 0;
                    break;
                case "DU":
                    XPos[m] = ((Order[m] - number_of_rows) * Stim_Size);
                    YPos[m] = screenYpixels;
                    break;
            }
        }
    }
}