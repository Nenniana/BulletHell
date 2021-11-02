using UnityEditor;

[CustomEditor(typeof(ProjectileUnit))]
public class AttachCameraEditor : Editor
{
    /*[Header("Misc. Settings")]
    public float weaponRange = 50f;
    public float hitForce = 100f;*/

    private bool playerSettings = false;
    private bool SpeedSettings = false;
    private bool DirectionSettings = false;
    private bool PlacementSettings = false;
    private bool speedSwitchSettings = false;
    private bool wallBounceSettings = false;
    private bool mouseFollowSettings = false;
    private bool PatternSettings = false;
    private static ProjectileUnit _target;


    public override void OnInspectorGUI()
    {
        _target = target as ProjectileUnit;

        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField("General Settings");

        EditorGUI.indentLevel++;
        _target.projectileObject = EditorGUILayout.ObjectField("ProjectileObject: ", _target.projectileObject, typeof(ProjectileObject), true) as ProjectileObject;
        _target.bulletsAmount = EditorGUILayout.IntSlider("Projectile Amount: ", _target.bulletsAmount, 0, 50);
        _target.spreadDistance = EditorGUILayout.Slider("Spread: ", _target.spreadDistance, -360, 360);
        _target.rotateSpeed = EditorGUILayout.Slider("Rotation Speed: ", _target.rotateSpeed, -400, 400);
        _target.shotsPerSecond = EditorGUILayout.IntSlider("Shots per Second: ", _target.shotsPerSecond, 0, 100);
        _target.shotSpeedOffset = EditorGUILayout.Slider("Shots-speed Offset: ", _target.shotSpeedOffset, 0, 10);
        _target.projectileSpeed = EditorGUILayout.Slider("Projt. Speed: ", _target.projectileSpeed, -10, 10);
        _target.lifeTime = EditorGUILayout.Slider("Projt. Lifetime: ", _target.lifeTime, 0, 20);
        _target.lifeDistance = EditorGUILayout.Slider("Projt. Life-distance: ", _target.lifeDistance, 0, 40);
        _target.rotate = EditorGUILayout.Toggle("Projt. Rotation: ", _target.rotate);
        EditorGUI.indentLevel--;

        playerSettings = EditorGUILayout.Foldout(playerSettings, "Player Settings");

        if (playerSettings || _target.playerTarget)
        {
            EditorGUI.indentLevel++;
            _target.playerTarget = EditorGUILayout.Toggle("Player Target: ", _target.playerTarget);
            EditorGUI.indentLevel--;
        }

        SpeedSettings = EditorGUILayout.Foldout(SpeedSettings, "Gradual Speed Settings");

        if (SpeedSettings || _target.speedUpper != 0 || _target.speedLower != 0)
        {
            EditorGUI.indentLevel++;
            _target.speedUpper = EditorGUILayout.Slider("Upper Speed: ", _target.speedUpper, 0, 30);
            _target.speedLower = EditorGUILayout.Slider("Lower Speed: ", _target.speedLower, 0, -30);
            _target.acceleration = EditorGUILayout.Slider("Acceleration: ", _target.acceleration, -10, 10);
            EditorGUI.indentLevel--;
        }

        PlacementSettings = EditorGUILayout.Foldout(PlacementSettings, "Directional Offset Settings");

        if (PlacementSettings || _target.offset != 0)
        {
            EditorGUI.indentLevel++;
            _target.offset = EditorGUILayout.Slider("Directional Offset: ", _target.offset, -360, 360);
            EditorGUI.indentLevel--;
        }

        DirectionSettings = EditorGUILayout.Foldout(DirectionSettings, "Switch Direction Settings");

        if (DirectionSettings || _target.directionSwitch)
        {
            EditorGUI.indentLevel++;
            _target.directionSwitch = EditorGUILayout.Toggle("Switch Direction: ", _target.directionSwitch);
            _target.directionSwitchPoint = EditorGUILayout.Slider("Switch Direction at: ", _target.directionSwitchPoint, -360, 360);
            EditorGUI.indentLevel--;
        }

        speedSwitchSettings = EditorGUILayout.Foldout(speedSwitchSettings, "Switch Speed Settings");

        if (speedSwitchSettings || _target.switchSpeedsContinue || _target.switchSpeedSpeed != 0)
        {
            EditorGUI.indentLevel++;
            _target.switchSpeedsContinue = EditorGUILayout.Toggle("Con Switch Speeds: ", _target.switchSpeedsContinue);
            _target.switchSpeedSpeed = EditorGUILayout.Slider("Switch Speed: ", _target.switchSpeedSpeed, 0, 10);
            _target.switchSpeedDistance = EditorGUILayout.Slider("First Switch Distance: ", _target.switchSpeedDistance, 0, 10);
            _target.switchSpeedDistanceSecond = EditorGUILayout.Slider("Second Switch Distance: ", _target.switchSpeedDistanceSecond, 0, 10);
            EditorGUI.indentLevel--;
        }

        mouseFollowSettings = EditorGUILayout.Foldout(mouseFollowSettings, "Follow Mouse Settings");

        if (mouseFollowSettings || _target.followMouse || _target.mouseTarget)
        {
            EditorGUI.indentLevel++;
            _target.mouseTarget = EditorGUILayout.Toggle("Emitter Follow Mouse: ", _target.mouseTarget);
            _target.followMouse = EditorGUILayout.Toggle("Projt. Follow Mouse: ", _target.followMouse);
            _target.MouseAsLastDirection = EditorGUILayout.Toggle("Set Mouse as Direction: ", _target.MouseAsLastDirection);
            _target.followAfterDistance = EditorGUILayout.Slider("Follow after Distance: ", _target.followAfterDistance, 0, 10);
            _target.followFromDistance = EditorGUILayout.Slider("Follow from Distance: ", _target.followFromDistance, 0, 10);
            EditorGUI.indentLevel--;
        }

        PatternSettings = EditorGUILayout.Foldout(PatternSettings, "Pattern Settings");

        if (PatternSettings || _target.ProjectileOrbits)
        {
            EditorGUI.indentLevel++;
            _target.ProjectileOrbits = EditorGUILayout.Toggle("Spawn Orbits: ", _target.ProjectileOrbits);
            _target.hideOriginalProjectile = EditorGUILayout.Toggle("Hide Original Projectile: ", _target.hideOriginalProjectile);
            _target.orbitRoationSpeed = EditorGUILayout.Slider("Orbit Rotation Speed: ", _target.orbitRoationSpeed, 0, 10);
            _target.orbitRadius = EditorGUILayout.Slider("Orbit Radius: ", _target.orbitRadius, 0, 10);
            _target.orbitCircle = EditorGUILayout.Slider("Orbit Circle: ", _target.orbitCircle, 0, 2);
            _target.numberOfOrbits = EditorGUILayout.IntSlider("Orbit Amount: ", _target.numberOfOrbits, 0, 100);
            EditorGUI.indentLevel--;
        }

        wallBounceSettings = EditorGUILayout.Foldout(wallBounceSettings, "Wall Bounce Settings");

        if (wallBounceSettings || _target.wallBounce)
        {
            EditorGUI.indentLevel++;
            _target.wallBounce = EditorGUILayout.Toggle("Wall Bounce?: ", _target.wallBounce);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel = 0;

        //If you want real custom inspector, you'd better not use default one at all or mark
        //fields that have to be hiden with [HideInInspector].
        //Also notice that DrawDefaultInspector () will cause duplication,
        //drawing those public serialized fields that you might already exposed with your custom editor.
        //DrawDefaultInspector ();
    }
}