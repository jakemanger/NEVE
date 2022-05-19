using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/// <summary>
/// Class for controlling movement of the animal via a UDP socket.
/// Is currently setup to use input in the format provided by fictrac
/// (see https://github.com/rjdmoore/fictrac), but can be easily modified.
/// </summary>
public class SocketMovementController : MonoBehaviour
{
    public bool recieveInputFromSocket = true;
    public int port = 1111;

    public float ballRadius = 60f;
    
    // variables for error checking
    int lastIndex = -1;

    Thread thread = null;

    public Vector3 targetPosition;
    Vector3 positionOffset;
    bool setPositionOffset = false;


    void Start()
    {
        targetPosition = transform.position; 
        if (recieveInputFromSocket) {
            thread = new Thread(new ThreadStart(GetSocketData));
            thread.IsBackground = true;
            thread.Start();
        }
    }

    void Update()
    {
        if (recieveInputFromSocket && setPositionOffset) {

            transform.position = targetPosition - positionOffset;

            // transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

        }
    }

    void GetSocketData()
    {
        UdpClient client = new UdpClient(port);
        while (true)
        {
            try
            {
                // receive message
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = client.Receive(ref remoteEndPoint);
                string receivedString = Encoding.ASCII.GetString(receiveBytes);

                // do something with message
                // print("Message received from the server: \n " + receivedString);
                MoveWithFictracInput(receivedString);
            }
            catch(Exception e)
            {
                Debug.LogWarning("Not using Closed Loop Input as Exception thrown: \n" + e.Message + "\n" + e.StackTrace);
            }
        }
    }


    /// <summary>
    /// Takes a string in the format provided by fictrac and converts it to a vector3
    /// then moves the animal in the direction of the vector.
    /// see info here about the format: https://github.com/rjdmoore/fictrac/blob/master/doc/data_header.txt
    /// </summary>
    void MoveWithFictracInput(string input)
    {
        // split the string by ',' and convert each element to a float
        string[] splitInput = input.Split(',');

        int index = int.Parse(splitInput[1]);

        if (lastIndex != index - 1)
        {
            int difference = lastIndex - index;
            Debug.LogWarning(
                "Missing " + difference.ToString()
                + " data points from fictrac, \n last index: "
                + lastIndex.ToString() + ", current index: " + index
            );
        }

        float x = (2f * (float)Math.PI * ballRadius) * (2f * float.Parse(splitInput[20]));
        float z = -(2f * (float)Math.PI * ballRadius) * (2f * float.Parse(splitInput[21]));

        targetPosition = new Vector3(x, targetPosition.y, z);

        if (!setPositionOffset) {
            positionOffset = targetPosition;
            setPositionOffset = true;
        }

        targetPosition += positionOffset;
        // print(index.ToString());

        lastIndex = index;
    }

    void OnDestroy() 
    {
        // make sure the thread is stopped
        if (thread != null)
        {
            thread.Abort();
        }
    }
}
