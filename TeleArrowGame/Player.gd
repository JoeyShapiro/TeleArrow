extends Area2D

export var speed = 400 # How fast the player will move (pixels/sec).
export(PackedScene) var arrow_scene
var charge = 0
export var has_arrow = true # TODO could use arrow instead of bool
export var reload = 1 # only so a new arrow is not spawned right away
export var arrow_collected = 0
var arrow # TODO return at different speeds
var screen_size # Size of the game window.
var main
var bar_charge


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	screen_size = get_viewport_rect().size
	main = get_node("../")
	bar_charge = get_node("BarCharge")


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
#	pass

func _process(delta):
	var velocity = Vector2.ZERO # The player's movement vector.
	if Input.is_action_pressed("move_right"):
		velocity.x += 1
	if Input.is_action_pressed("move_left"):
		velocity.x -= 1
	if Input.is_action_pressed("move_down"):
		velocity.y += 1
	if Input.is_action_pressed("move_up"):
		velocity.y -= 1
		
	if Input.is_action_pressed("arrow"):
		if has_arrow:
			charge += 0.01
			bar_charge.value = charge * 100
			charge = clamp(charge, 0, 1.0)
		else:
			arrow.come_back(self, true)
	
	if Input.is_action_just_released("arrow") && !has_arrow:
		arrow.come_back(self, false)
	elif Input.is_action_just_released("arrow") && has_arrow && arrow_collected <= 0:
		shoot()
	if arrow_collected > 0:
		arrow_collected -= delta

	if velocity.length() > 0:
		velocity = velocity.normalized() * speed
		
	position += velocity * delta
	position.x = clamp(position.x, 0, screen_size.x)
	position.y = clamp(position.y, 0, screen_size.y)

func collect():
	has_arrow = true
	arrow_collected = reload

func shoot():
	arrow = arrow_scene.instance()
	arrow.position = position
	
	var r = get_angle_to(get_global_mouse_position())
	arrow.direction = Vector2(cos(r), sin(r))
	
	arrow.speed = 1200 * charge
	charge = 0
	main.add_child(arrow)
	has_arrow = false
