extends Area2D

# Declare member variables here. Examples:
# var a: int = 2
# var b: String = "text"
export var speed_max = 0
export var fall_off = 0
var speed
export var direction = Vector2.ZERO
export var come = false
var player_ref
var tal # time alive
var stuck
var stuck_to

var screen_size # Size of the game window.

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	screen_size = get_viewport_rect().size
	speed = speed_max
	tal = 0

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	tal += delta
	
	if come:
		var r = get_angle_to(player_ref.position) + rotation
		direction = Vector2(cos(r), sin(r)) * speed_max
		position += direction * delta
		if position.distance_to(player_ref.position) < 10:
			player_ref.collect()
			queue_free()
	else:
		direction = direction.normalized() * speed
		position += direction * delta
	
	# if stuck on another player
	if stuck:
		position = stuck_to.position
	
	# decrease the speed (like its dropping)
	speed = speed_max * pow(exp(1), fall_off*tal*15)

func _on_VisibilityNotifier2D_screen_exited():
	queue_free()

func come_back(player, should): # TODO player should be on init
	stuck = false
	direction = Vector2.ZERO
	come = should
	player_ref = player


func _on_Arrow_body_entered(body: Node) -> void:
	direction = Vector2.ZERO
	stuck = true
	stuck_to = body
