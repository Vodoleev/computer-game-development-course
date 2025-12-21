
## Автор
ФИО: Подолеев Вадим Романович
Группа: 2211

## Сцены (точки входа)
- MenuScene — стартовая сцена
- GameScene — основной игровой процесс

## Управление
- Влево/вправо: ← → (или A/D)
- Прыжок: Space / ↑ / W
- Пауза: ESC

## Реализовано (HW2)
- Главное меню (название, автор, рекорд, Start/Quit)
- HUD в игре: HP, Score, HighScore, скорость, сообщения о бонусах
- Пауза по ESC + Resume/Exit to Menu
- Экран Game Over + Restart/Exit
- Сохранение High Score (между запусками)
- Бонусы: лечение и ускорение
- Анимации через Animator:
  - DamageFlash (Trigger `Damage`)
  - BonusFlash (Trigger `Bonus`)
  - SpeedBoostLoop (Bool `SpeedBoostActive`)

## Отличия от HW1
- Добавлены меню/пауза/game over/рекорд/анимации/вывод параметров на экран.
