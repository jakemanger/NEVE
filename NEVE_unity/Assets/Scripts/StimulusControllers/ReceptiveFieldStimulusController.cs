using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceptiveFieldStimulusController : GenericStimulusController
{
    public PathRandomization path;
    public Image image;
    public Image bgImage;
    public float speed;
    private int currentIndex;
    private Vector2 currentPosition;

    public Color imageColour = Color.white;
    public Color bgColour = Color.black;

    public int number_of_columns;
    public int number_of_rows;
    public float Stim_Size;
    public int screenYpixels;
    public int screenXpixels;
    public int seed;

    public override void Reset() {
        image = GetComponent<Image>();
        path = GetComponent<PathRandomization>();
        path.number_of_columns = number_of_columns;
        path.number_of_rows = number_of_rows;
        path.Stim_Size = Stim_Size;
        path.screenYpixels = screenYpixels;
        path.screenXpixels = screenXpixels;
        path.seed = seed;
        path.Initialise();

        currentIndex = 0;
        currentPosition = new Vector2(path.XPos[currentIndex], path.YPos[currentIndex]);

        image.color = imageColour;
        bgImage.color = bgColour;
    }

    void Update() {
        stimulusState = StimulusState.Started;
        currentPosition = Vector2.MoveTowards(currentPosition, new Vector2(path.XPos[currentIndex], path.YPos[currentIndex]), speed * Time.deltaTime);
        if (currentPosition == new Vector2(path.XPos[currentIndex], path.YPos[currentIndex])) {
            currentIndex++;
            if (currentIndex >= path.Order.Length) {
                currentIndex = 0;
            }
        }
        DrawPixel(currentPosition);
    }

    void DrawPixel(Vector2 position) {
        // print("Drawing pixel at " + position.ToString());
        Vector2 maxBounds = new Vector2(path.Stim_Size * path.number_of_rows, path.Stim_Size * path.number_of_columns);
        image.rectTransform.localPosition = position - maxBounds / 2;
        image.rectTransform.sizeDelta = new Vector2(path.Stim_Size, path.Stim_Size);
        bgImage.rectTransform.sizeDelta = maxBounds;
    }
}
