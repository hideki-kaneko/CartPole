using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CartAgent : Agent
{
    // Public
    public float Speed = 1;
    public GameObject mObjPole;
    public float randomInitPosXMax = 1;
    public float initPosY = 0;
    public float worstAngleDeg = 90;

    // Private
    private Vector3 initDir;

    // ランダムな初期位置をセットする
    public void SetRandomCartPosition()
    {
        float initX = Random.Range(-randomInitPosXMax, randomInitPosXMax);
        Vector3 initPos = new Vector3(initX, initPosY, 0);
        Rigidbody rigidbodyCart = GetComponent<Rigidbody>();
        rigidbodyCart.transform.position = initPos;
    }

    // 傾きによる報酬を計算する
    public float CalcAngleReward()
    {
        float angle = GetPoleAngle();
        float reward = Mathf.Clamp(1 - angle / worstAngleDeg, 0, 1);
        return reward;
    }

    // カートの状態を取得
    public float GetCartPosition()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        return rigidbody.position.x;
    }
    public float GetCartVelocity()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        return rigidbody.velocity.x;
    }

    // ポールの状態を取得
    public float GetPoleAngle()
    {
        Rigidbody rigidbodyPole = mObjPole.GetComponent<Rigidbody>();
        Vector3 currentDir = rigidbodyPole.transform.up;
        float rotDeg = Vector3.Angle(currentDir, initDir);
        return rotDeg;
    }
    public float GetPoleAngularVelocity()
    {
        Rigidbody rigidbody = mObjPole.GetComponent<Rigidbody>();
        return rigidbody.angularVelocity.z;
    }
    public bool IsPoleFallen()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if ((GetPoleAngle() >= worstAngleDeg) || rigidbody.position.y <= 0)
        {
            return true;
        }

        return false;
    }

    // GUIを描画する
    private void drawGUI_()
    {
        GUI.Box(new Rect(10, 10, 140, 150), "Info");
        GUI.Label(new Rect(20, 30, 100, 90), string.Format("CartPosition:{0:F1}", GetCartPosition()));
        GUI.Label(new Rect(20, 50, 100, 90), string.Format("CartVelocity:{0:F1}", GetCartVelocity()));
        GUI.Label(new Rect(20, 70, 100, 90), string.Format("PoleAngle:{0:F1}", GetPoleAngle()));
        GUI.Label(new Rect(20, 90, 100, 90), string.Format("PoleAngularVel:{0:F1}", GetPoleAngularVelocity()));
        GUI.Label(new Rect(20, 110, 100, 90), string.Format("Reward:{0:F1}", CalcAngleReward()));
        GUI.Label(new Rect(20, 130, 100, 90), string.Format("Fail:{0}", IsPoleFallen().ToString()));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void AgentReset()
    {
        base.AgentReset();

        SetRandomCartPosition();
        Rigidbody rigidbodyPole = mObjPole.GetComponent<Rigidbody>();
        initDir = rigidbodyPole.transform.up;
    }

    public override void CollectObservations()
    {
        base.CollectObservations();

        AddVectorObs(GetCartPosition());
        AddVectorObs(GetCartVelocity());
        AddVectorObs(GetPoleAngle());
        AddVectorObs(GetPoleAngularVelocity());
    }

    public override void AgentAction(float[] vectorAction)
    {
        base.AgentAction(vectorAction);

        Vector3 inputVector = Vector3.zero;
        inputVector.x = vectorAction[0];
        Vector3 inputForce = inputVector * Speed;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(inputForce);

        if (IsPoleFallen())
        {
            SetReward(-1);
            Done();
        } else
        {
            float reward = CalcAngleReward();
            SetReward(reward);
        }
    }
}
