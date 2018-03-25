using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekPlusArriveCollTugas: MonoBehaviour
{

    /**

data kinematic : posisi dan orientasi pada karakter telah terdapat pada class transform
untuk mengakses gunakan transform
    **/


    //Transform charakter; // sudah tidak perlu karena sudah bisa diakses dengan syntax this.transform 
    public Transform _target;
    public int _maxSpeed;
    public int _radius;
    public int _timeToTarget;
    public int _maxAvoidanceForce;
    public int _maxAhead;
    public Vector3 _currentVelocity;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //GLB
        //position = position + velocity
        
        this.transform.position = (transform.position + getSteering()._velocity)*Time.deltaTime+(this.transform.position+getCollideAvoid()._velocity) * Time.deltaTime;
        _currentVelocity = getSteering()._velocity;
        
     }

    public KinematicData getCollideAvoid()
    {
        Vector3 ahead = this.transform.position + _currentVelocity.normalized * _maxAhead * 0.5f;

        CircleObs _ClosestObsCirle = getClosetObstacle();
        KinematicData _SteeringOut = new KinematicData();

        //lengkapi .... yang dibawah ini

        if (_ClosestObsCirle != null)
        {
            _SteeringOut._velocity = ahead - _ClosestObsCirle._center;
            if (_SteeringOut._velocity.magnitude > _maxAvoidanceForce)
            {
                _SteeringOut._velocity = _SteeringOut._velocity.normalized;
                _SteeringOut._velocity *= _maxAvoidanceForce;
            }
        }
        else
        {
            _SteeringOut._velocity = Vector3.zero;
        }
        return _SteeringOut;
    }

    bool lineIntersecCircle(Vector3 ahead, Vector3 ahead2, CircleObs obstacle) {

        float distance1 =( obstacle._center - ahead).magnitude;
        float distance2 = (obstacle._center - ahead2).magnitude;
        return (distance1 <= obstacle._radius)|| (distance2<=obstacle._radius) ;
    }

    public CircleObs getClosetObstacle() {
        Vector3 ahead = this.transform.position + _currentVelocity.normalized * _maxAhead;
        Vector3 ahead2 = this.transform.position + _currentVelocity.normalized * _maxAhead * 0.5f;
        CircleObs mostThreatening = null;
        foreach (GameObject _obstacle in GameObject.FindGameObjectsWithTag("Obstacle")) {
            CircleObs circleObs = _obstacle.GetComponent<CircleObs>();
            bool _isColide = lineIntersecCircle(ahead, ahead2, circleObs);
            if (_isColide) {
                if (mostThreatening == null)
                {
                    mostThreatening = circleObs;
                }
                else
                {
                    float distanceCurrent = (this.transform.position - mostThreatening._center).magnitude;
                    float distanceCek = (this.transform.position - circleObs._center).magnitude;
                    mostThreatening = distanceCek < distanceCurrent ? circleObs : mostThreatening;
                }
            }
        }
        
        return mostThreatening;
    }
    
    public KinematicData getSteering()
    {
        KinematicData _KinematicOut = new KinematicData();
        _KinematicOut._velocity = _target.position - this.transform.position; //#direction

        if (_KinematicOut._velocity.magnitude < _radius) //jika di dalam radius velocity = 0; berhenti
        {
            _KinematicOut._velocity = Vector3.zero; //jika di dalam radius velocity = 0; berhenti
            _KinematicOut._rotation = Vector3.zero;
            return _KinematicOut;
         }

        _KinematicOut._velocity /= _timeToTarget; //agar datang sesuai waktu yg di tentukan 

        if (_KinematicOut._velocity.magnitude > _maxSpeed)
        {
            _KinematicOut._velocity = _KinematicOut._velocity.normalized; // normalize membuat resultan vektor = 1.
            _KinematicOut._velocity *= _maxSpeed;
        }
        //this.transform.eulerAngles = getNewOrientation(this.transform.eulerAngles, _KinematicOut._velocity); //rotation menggunakan euler angle
        _KinematicOut._rotation = Vector3.zero;
        return _KinematicOut;
    }

    public Vector3 getNewOrientation(Vector3 _currentOrientation, Vector3 _velocity)
    {
        //Make sure we have a velocity
        if (_velocity.magnitude > 0) //length didapat dri fungsi magnitude (mendapatkan resultan)
        {
            //Calculate orientation using an arc tangent of the velocity components.
            float radian = Mathf.Atan2(-_velocity.x, _velocity.z); //dalam radian
            //float angle = radian * (180/Mathf.PI);  // untuk mengganti ke angle
            Vector3 newOrientation = new Vector3(0, radian, 0);
            return newOrientation;
        }

        //Otherwise use the current orientation
        else
            return _currentOrientation;


    }






}
