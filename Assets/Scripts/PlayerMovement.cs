using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Problems:
    /// isGrounded geeft vaak false negatives

    public CharacterController Controller;
    public Transform player;
    _Cam = Camera.Main; //Changed not Tested

    public float MovementSpeed;
    public float sprintMultiplier;
    public float g;
    public float jumpHeight;
    public float massPlayer;

    public float FOVShiftTime;
    public float FOVSprint;
    public float FOVNormal;
    public Transform gunParent;

    //private Vector3 jumpMovement;
    private Vector3 verticalMove;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 horizontalMove = transform.right * x + transform.forward * z;


        bool isGrounded = GetComponent<CharacterController>().SimpleMove(horizontalMove);
        if (isGrounded)
        {
            verticalMove.y = 0f; //prevents player from falling forever
            if (Input.GetButton("Jump"))
            {
                verticalMove.y = jumpHeight;
                //jumpMovement = horizontalMove;
            } //jumps



            if (Input.GetKey("left shift"))
            {
                horizontalMove = new Vector3(horizontalMove.x * sprintMultiplier, 0, horizontalMove.z * sprintMultiplier);
                _Cam.fieldOfView = Mathf.Lerp(_Cam.fieldOfView, FOVSprint, FOVShiftTime);
            } //sprints
            else
            {
                _Cam.fieldOfView = Mathf.Lerp(_Cam.fieldOfView, FOVNormal, FOVShiftTime);
            } //doesn't sprint

        }
        else
        {
            //Debug.Log("ungrounded");
            verticalMove.y -= g * massPlayer * Time.deltaTime;
            //horizontalMove = jumpMovementduszegmaaraangepastesnelheidomdatjenietkuntversnellenindeluchthaha;
        }



        Controller.Move(horizontalMove * MovementSpeed * Time.deltaTime + verticalMove * Time.deltaTime);
    }
}