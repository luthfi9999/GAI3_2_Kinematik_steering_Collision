using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerSeek : MonoBehaviour
{

    
 /**

data kinematic : posisi dan orientasi pada karakter telah terdapat pada class transform
untuk mengakses gunakan transform
      **/
   


    //Transform charakter; // sudah tidak perlu karena sudah bisa diakses dengan syntax this.transform 
    public Transform _target;
    public int _maxSpeed;
    public int _maxAcceleration;
    public Vector3 _currentVelocity = Vector3.zero;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        //if (_currentVelocity.magnitude > _maxSpeed)
        //{
        //    _currentVelocity = _currentVelocity.normalized * _maxSpeed;
        //    // sudah makspeed maka menggunakan GLB
        //    this.transform.position = transform.position + _currentVelocity * Time.deltaTime;
        //}
        //else
        //{
        //    //GLBB
        //    this.transform.position = transform.position + _currentVelocity * Time.deltaTime + 0.5f*getSteering()._linear * Time.deltaTime* Time.deltaTime;
        //}

        this.transform.position = transform.position + _currentVelocity * Time.deltaTime;
        _currentVelocity = _currentVelocity + getSteering()._linear * Time.deltaTime; // Vt = Vo +a.t
         if (_currentVelocity.magnitude > _maxSpeed)
        {
            _currentVelocity = _currentVelocity.normalized * _maxSpeed;
        
        }
        

    }

    public SteeringData getSteering()
    {
        SteeringData _SteeringOut = new SteeringData();
        _SteeringOut._linear = _target.position - transform.position; //#direction
        _SteeringOut._linear = _SteeringOut._linear.normalized; // normalize membuat resultan vektor = 1.
        _SteeringOut._linear *= _maxAcceleration;
        // this.transform.eulerAngles = getNewOrientation(this.transform.eulerAngles, _KinematicOut._velocity); //rotation menggunakan euler angle
        _SteeringOut._angular = Vector3.zero; //orientasi belum kita hitung
        return _SteeringOut;
    }

    public Vector3 getNewOrientation(Vector3 _currentOrientation, Vector3 _velocity)
    {
        //Make sure we have a velocity
        if (_velocity.magnitude > 0) //length didapat dri fungsi magnitude (mendapatkan resultan)
        {
            //Calculate orientation using an arc tangent of the velocity components.
            float radian = Mathf.Atan2(_velocity.x, _velocity.z); //dalam radian, tidak perlu minus karena rotasi unity searah jarum jam
            float angle = radian * (180 / Mathf.PI);  // untuk mengganti ke angle
            Vector3 newOrientation = new Vector3(0, angle, 0);
            return newOrientation;
        }

        //Otherwise use the current orientation
        else
            return _currentOrientation;

    }






}
