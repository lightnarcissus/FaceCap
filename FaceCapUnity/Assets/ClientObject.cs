using System.Collections.Concurrent;
using System.Threading;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;

public class NetMqListener
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate void MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var subSocket = new SubscriberSocket())
        {
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect("tcp://localhost:12345");
            subSocket.Subscribe("");
            while (!_listenerCancelled)
            {
                string frameString;
                if (!subSocket.TryReceiveFrameString(out frameString)) continue;
                Debug.Log(frameString);
                _messageQueue.Enqueue(frameString);
            }
            subSocket.Close();
        }
        NetMQConfig.Cleanup();
    }

    public void Update()
    {
        while (!_messageQueue.IsEmpty)
        {
            string message;
            if (_messageQueue.TryDequeue(out message))
            {
                _messageDelegate(message);
            }
            else
            {
                break;
            }
        }
    }

    public NetMqListener(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _listenerWorker = new Thread(ListenerWork);
    }

    public void Start()
    {
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

    public void Stop()
    {
        _listenerCancelled = true;
        _listenerWorker.Join();
    }
}

public class ClientObject : MonoBehaviour
{
    private NetMqListener _netMqListener;

    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    private void HandleMessage(string message)
    {
        Debug.Log("handled message is " + message);
        var splittedStrings = message.Split(',');
        //UnityEngine.Debug.Log("the val is " + message.ToString());

        float val = float.Parse(splittedStrings[1]) * 20f;
        Debug.Log("val is " + val.ToString());
        if (splittedStrings[0] == "AU02")
        {
            skinnedMeshRenderer.SetBlendShapeWeight(8, val);
            skinnedMeshRenderer.SetBlendShapeWeight(9, val);
        }
        else if(splittedStrings[0]=="AU06")
        {
            skinnedMeshRenderer.SetBlendShapeWeight(41, val);
            skinnedMeshRenderer.SetBlendShapeWeight(42, val);
        }
        else if (splittedStrings[0] == "AU12")
        {
            skinnedMeshRenderer.SetBlendShapeWeight(41, val);
            skinnedMeshRenderer.SetBlendShapeWeight(42, val);
        }
        else if (splittedStrings[0] == "AU25")
        {
            skinnedMeshRenderer.SetBlendShapeWeight(35, val);
            //skinnedMeshRenderer.SetBlendShapeWeight(42, val);
        }
        else if (splittedStrings[0] == "AU45")
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, val);
            skinnedMeshRenderer.SetBlendShapeWeight(1, val);
        }
    }

    private void Awake()
    {

        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }
    private void Start()
    {
        _netMqListener = new NetMqListener(HandleMessage);
        _netMqListener.Start();
    }

    private void Update()
    {
        _netMqListener.Update();
    }

    private void OnDestroy()
    {
        _netMqListener.Stop();
    }
}
