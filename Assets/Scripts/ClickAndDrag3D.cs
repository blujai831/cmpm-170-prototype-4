using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ClickAndDrag3D : MonoBehaviour
{
    [SerializeField] private float _mouseDragAggression;
    [SerializeField] private float _scrollSensitivity;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _dragging;
    private Vector3 _positionClicked;
    private float _cameraDistanceClicked;
    private Vector3 _lastKnownMousePosition;
    private RaycastHit _raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _dragging = false;
        _raycastHit = new RaycastHit();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var selected = UpdateMousePositionAndReturnWhetherSelected();
        var mousePressed = Mouse.current.leftButton.isPressed;
        var mousePressedThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
        if (!mousePressed && !mousePressedThisFrame) {
            _dragging = false;
        } else if (_dragging) {
            var worldPosnClicked = transform.TransformPoint(_positionClicked);
            _rigidbody.AddForceAtPosition(
                DeltaPositionToForce(
                    _lastKnownMousePosition - worldPosnClicked,
                    Time.deltaTime
                ),
                worldPosnClicked
            );
        } else if (selected && mousePressedThisFrame) {
            _dragging = true;
            _positionClicked =
                transform.InverseTransformPoint(_raycastHit.point);
            _cameraDistanceClicked = (
                _raycastHit.point - Camera.main.transform.position
            ).magnitude;
        }
    }

    private bool UpdateMousePositionAndReturnWhetherSelected() {
        var selected = false;
        var oldMousePosition = _lastKnownMousePosition;
        var screenSpacePosn = new Vector3(
            Mouse.current.position.x.ReadValue(),
            Mouse.current.position.y.ReadValue(),
            0.0f
        );
        if (_dragging) {
            _cameraDistanceClicked +=
                Mouse.current.scroll.ReadValue().y*_scrollSensitivity;
            screenSpacePosn.z = _cameraDistanceClicked;
            _lastKnownMousePosition =
                Camera.main.ScreenToWorldPoint(screenSpacePosn);
            selected = true;
        } else {
            var comparisonPosn = new Vector3(
                screenSpacePosn.x,
                screenSpacePosn.y,
                1.0f
            );
            var naiveWorldSpacePosn =
                Camera.main.ScreenToWorldPoint(screenSpacePosn);
            var naiveWorldSpaceComparisonPosn =
                Camera.main.ScreenToWorldPoint(comparisonPosn);
            var direction =
                naiveWorldSpaceComparisonPosn - naiveWorldSpacePosn;
            if (Physics.Raycast(
                naiveWorldSpacePosn, direction, out _raycastHit
            ) && _raycastHit.collider == _collider) {
                _lastKnownMousePosition = _raycastHit.point;
                selected = true;
            } else {
                _lastKnownMousePosition = naiveWorldSpacePosn;
            }
        }
        return selected;
    }

    private Vector3 DeltaPositionToForce(
        Vector3 delta, float deltaTime
    ) {
        return _mouseDragAggression*2.0f*(
            delta - deltaTime*_rigidbody.velocity
        )/(deltaTime*deltaTime);
    }
}
