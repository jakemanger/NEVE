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

    // a minimum distance for the movement to be registered
    public float minMovementDistance = 0f;

    public float maxDistanceDelta = 0.1f;
    
    // variables for error checking
    int lastIndex = -1;
    int n_socket_updates = 0;
    float t_since_last_ups = 0f;
    public bool verbose = false;
    public bool displayUPS = false;

    Thread thread = null;

    public Vector3 targetPosition;
    Vector3 positionOffset;
    bool setPositionOffset = false;

    public float xMultiplier = -1f;
    public float zMultiplier = 1f;

    public void Reset()
    {
        Vector3 initialPos = new Vector3(0f, transform.position.y, 0f);
        positionOffset = targetPosition;
        targetPosition = initialPos;
        transform.position = initialPos;
    }


    void Start()
    {
        targetPosition = transform.position; 
        if (recieveInputFromSocket)
        {
            StartThread();
        }
    }

    void StartThread()
    {
        if (thread == null)
        {
            thread = new Thread(new ThreadStart(GetSocketData));
            thread.IsBackground = true;
            thread.Start();
        }

    }

    void Update()
    {
        if (recieveInputFromSocket && setPositionOffset)
        {
            if (Vector3.Distance(transform.position, targetPosition) > minMovementDistance)
            {
                if (maxDistanceDelta > 0)
                {
                    // use MoveTowards
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition - positionOffset, maxDistanceDelta * Time.deltaTime);
                }
                else
                {
                    transform.position = targetPosition - positionOffset;
                }
            }
        }

        // calculate updates per second every second
        if (displayUPS)
        {
            if (t_since_last_ups > 1f)
            {
                t_since_last_ups = 0f;
                Debug.Log("Socket updates per second: " + n_socket_updates);
                n_socket_updates = 0;
            }
            else
            {
                t_since_last_ups += Time.deltaTime;
            }
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
                if (verbose)
                {
                    print("Message received from the server: \n " + receivedString);
                }

                MoveWithFictracInput(receivedString);
                n_socket_updates++;
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

        float z = ballRadius * float.Parse(splitInput[20]);
        float x = ballRadius * float.Parse(splitInput[21]);

        targetPosition = new Vector3(x * xMultiplier, targetPosition.y, z * zMultiplier);

        if (!setPositionOffset)
        {
            positionOffset = targetPosition;
            setPositionOffset = true;
            if (verbose)
            {
                print("position offset = " + positionOffset);
            }
        }
        if (verbose)
        {
            print("target position = " + targetPosition);
        }


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
