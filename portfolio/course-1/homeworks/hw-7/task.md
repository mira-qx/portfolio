Домашнее задание

Уровень 1

Задание 1

Создайте следующую структуру классов:
1. Базовый класс Transport
Свойства:
public string Model { get; set; }
protected int Speed — скорость (начальное значение 0)


Методы:
public void ShowInfo() — выводит:
 "Модель: {Model}, скорость: {Speed} км/ч"
public virtual void Move() — выводит "Транспорт движется".

2. Класс Car, наследник Transport
Добавьте:
метод public void Accelerate(int value), который увеличивает Speed на value, но не более чем до 200. (Если значение больше — выведите сообщение "Слишком большая скорость!")
переопределите метод Move() (override) → "Машина едет по дороге".

3. Класс Bicycle, наследник Transport
Добавьте:
метод public void Pedal() — увеличивает Speed на 5;
переопределите метод Move() → "Велосипед крутит педали".

Пример использования:
var car = new Car { Model = "Audi" };
car.Accelerate(100);
car.ShowInfo();
car.Move();

var bike = new Bicycle { Model = "Cube" };
bike.Pedal();
bike.ShowInfo();
bike.Move();


Ожидаемый результат:
Модель: Audi, скорость: 100 км/ч
Машина едет по дороге
Модель: Cube, скорость: 5 км/ч
Велосипед крутит педали


Уровень 2

Задание 2

1. Создайте абстрактный класс Character
Содержит:
свойство Name;
абстрактный метод:


public abstract void Attack();


2. Создайте наследников
Каждый реализует метод Attack():
Warrior — "Воин атакует мечом!"
Mage — "Маг выпускает огненный шар!"
Archer — "Лучник стреляет из лука!"


Пример:
Character[] team =
{
    new Warrior { Name = "Алекс" },
    new Mage { Name = "Лия" },
    new Archer { Name = "Робин" }
};

foreach (var c in team)
{
    Console.Write($"{c.Name}: ");
    c.Attack();
}

