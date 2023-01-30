extends RigidBody2D

# Declare member variables here. Examples:
# var a: int = 2
# var b: String = "text"
export var speed = 0
export var direction = Vector2.ZERO
export var come = false
var player_ref

var screen_size # Size of the game window.

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	screen_size = get_viewport_rect().size

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if come:
		var r = get_angle_to(player_ref.position) + rotation
		direction = Vector2(cos(r), sin(r)) * speed
		position += direction * delta
		if position.distance_to(player_ref.position) < 10:
			player_ref.collect()
			queue_free()
	else:
		direction = direction.normalized() * speed
		position += direction * delta

func _on_VisibilityNotifier2D_screen_exited():
	queue_free()

func come_back(player, should): # TODO player should be on init
	direction = Vector2.ZERO
	come = should
	player_ref = player
