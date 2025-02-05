# Core API

### Версия: **0.3**

### Оглавление

- [Правила](#правила)
- [Начало](#начало)
- [Composition Root](#composition-root)
- [Prefab Control](#prefab-control)
- [Misc](#misc)

**[Назад](../README.md)**

## Правила

Чтобы правильно использовать Core API в первую очередь нужно следовать нескольким основным правилам:

1. Не менять архитектуру папок и распределять файлы в соответствии с ней.
2. Ничего не изменять в папке **Scripts/Core**.
3. Весь код должен содержаться в пространстве имён **TanksArmageddon** или в его подпространствах, таких как **TanksArmageddon.Example**.
4. Весь функционал Core находится в пространстве имён **TanksArmageddon.Core** или его подпространствах, добавление новых элементов в это пространство запрещено.
5. Core объединяет в себе различные модули, упрощающие работу с Unity, предпочтительнее использовать их, когда это возможно.

## Начало

В зависимости он задач ты можешь использовать один или несколько модулей из Core:

- **CompositionRoot** (TanksArmageddon.Core.CompositionRoot)
- **PrefabControl** (TanksArmageddon.Core.PrefabControl)
- **Misc** (TanksArmageddon.Core)

## Composition Root

Это система инициализации (внедрения зависимостей), она поддерживает 2 режима использования:

1. **Инициализация отдельных объектов**
2. **Конструирование сцены с нуля** (требуется также Prefab Control)

### Правила

- Чтобы работать с данным модулем нужно подключать пространство имён **TanksArmageddon.Core.CompositionRoot**.
- Все скрипты, что должны быть размещены на объектах на сцене должны наследоваться от класса **MonoScript**.
- Для корректной работы модуля запрещается использование методов **Start** и **Awake**.
- Заместо метода **Awake** нужно использовать **Construct**, при этом он может быть с параметрами или без них.
- Заместо метода **Start** нужно использовать **LazyConstruct**, при этом он также может быть с параметрами или без них, а также его особенность заключается в поддержке асинхронного кода.
- Для автоматического создания метода **Construct** или **LazyConstruct**, можно временно реализовать интерфейс **IConstructible** или **ILazyConstructible** с помощью Visual Studio.
- На всю сцену должен быть лишь один **CompositionRoot**.

### Инициализация

На сцене должен находиться один объект, на который будет повешен собственный класс, унаследованный от **BaseCompositionRoot**, при этом на этот класс нужно повесить атрибут **DefaultExecutionOrder** с переданной в него константой **ExecutionOrderValue**.

```
[DefaultExecutionOrder(ExecutionOrderValue)]
public class TestCompositionRoot : BaseCompositionRoot
{
}
```

В этом классе мы будем конструировать группы, для этого мы можем переопределить 3 метода, по необходимости

```
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class TestCompositionRoot : BaseCompositionRoot
    {
        public override void CreateEnvironment()
        {
        }

        public override void Construct()
        {
        }

        public override IEnumerator LazyConstruct()
        {
            yield return null;
        }
    }
```

**CreateEnvironment** - для создания простых объектов, не требующих инициализации (декор, камера, освещение).

**Construct** - для настройки/создания сложных объектов имеющих на себе скрипты, требующие инициализации.

**LazyConstruct** - тоже что и **Construct**, но для работы с асинхронным кодом.

Теперь нужно создать одну или несколько групп, в которых будут инициализироваться объекты. Каждая группа должна унаследовать класс **BaseCompositionGroup** и, при необходимости, переопределить те же три метода, что и у **CompositeRoot**.

Группы должны быть расположены или на том же объекте что и **CompositeRoot** или на дочерних объектах.

```
    public class FirstCompositionGroup : BaseCompositionGroup
    {
        public override void CreateEnvironment()
        {
        }

        public override void Construct()
        {
        }
    }
```

Теперь есть 2 варианта, как передать эту группу на инициализацию в **CompositeRoot**:

Для особых групп (групп, имеющих объекты, требующие в качестве зависимостей объекты из других групп), необходимо создать в **CompositeRoot** отдельное сериализованное поле нужного типа группы и передать эту группу через инспектор. После чего можно вызвать соответствующие методы группы в методах **CompositeRoot**.

```
    [DefaultExecutionOrder(ExecutionOrderValue)]
    public class TestCompositionRoot : BaseCompositionRoot
    {
        [SerializeField] private FirstCompositionGroup _firstCompositionGroup;

        public override void CreateEnvironment()
        {
            _firstCompositionGroup.CreateEnvironment();
        }

        public override void Construct()
        {
            _firstCompositionGroup.Construct();
        }

        public override IEnumerator LazyConstruct()
        {
            yield return _firstCompositionGroup.LazyConstruct();
        }
    }
```

Инициализацией особых объектов занимается сам **CompositeRoot**, получая нужные объекты через свойства у групп.

Для обычных групп всё проще, их просто нужно перенести в список **Other Groups**, что отображается в инспекторе у компонента **CompositeRoot**, они автоматически проинициализируются после особых в порядке размещения в списке

В самих группах инициализация происходит очень просто, объект передаётся через сериализованное поле в группу где инициализируется в нужных методах или создаётся внутри группы, также получая префаб через поле.

```
    public class FirstCompositionGroup : BaseCompositionGroup
    {
        [SerializeField] private TestObject1 _testObject1;

        public override void CreateEnvironment()
        {
        }

        public override void Construct()
        {
            _testObject1.Construct("text");
        }
    }
```

При написании класса которому требуется инициализация мы наследуем его от **MonoScript**, пишем метод **Construct** или **LazyConstruct** с параметрами или без, можно создать через Visual Studio, временно реализовав интерфейс **IConstructible** или **ILazyConstructible**. Важно в конце каждого из этих методов вызвать соответствующий обработчик события: **OnConstructed** или **OnLazyConstructed**.

```
    public class TestObject1 : MonoScript
    {
        public void Construct(string text = "default")
        {
            Debug.Log("constructed");
            Debug.Log(text);

            OnConstructed();
        }

        public IEnumerator LazyConstruct()
        {
            yield return null;

            OnLazyConstructed();
        }
    }
```

### Создание объектов

⚠️ **Это тестовая версия API, которая может измениться со временем** ⚠️

Если нужно создать объект, требующий инициализации, **не используй Instantiate**, для этого существует класс **ObjectBuilder** и метод **CreateNew**.

Пользоваться **ObjectBuilder** может показаться сложно, но тут главное понять принцип, **CreateNew** имеет несколько перегрузок, базовые - 2, остальные для интеграции с **Prefab Control**, разберём их:

### 1-я перегрузка

```
T CreateNew<T>(T prefab, Func<T> creator)
```

1-й параметр: предай префаб, но не как **GameObject**, а как класс того скрипта, что на нём находится, если их несколько выбираешь нужный тебе.

2-й параметр: это функция создания, тебе просто нужно написать обычный **Instantiate**, с любыми параметрами, что позволяет Unity и поставить перед ним **() =>**

Важно, этот метод вернёт тебе компонент нужного типа (что и 1-й параметр) с созданного объекта на сцене, при этом сам объект будет выключен, это нужно, чтобы ты мог вызвать конструктор и только после этого вручную его включить, после включения у объекта сработает **OnEnable**.

Пример использования:

```
TestObject1 testObject1 = ObjectBuilder.CreateNew(
    _testObject1Prefab, () => Instantiate(_testObject1Prefab, transform));

testObject1.Construct("text");
```

в данном случае **\_testObject1Prefab** хоть и префаб, но имеет тип **TestObject1**, так как мы передали его через поле таким образом:

```
[SerializeField] private TestObject1 _testObject1Prefab;
```

### 2-я перегрузка

```
T CreateNew<T>(T prefab, Func<T> creator, Action<T> initializer, bool isActivateObject = true)
```

1-й и 2-й параметры такие же как и в 1-й перегрузке

3-й параметр: эта функция, что используется для инициализации созданного объекта, иными словами вызов конструктора у этого объекта, тут немного сложнее:
`(<Имя параметра, как для метода>) => параметр.Construct(<какие-то аргументы>)`.

4-й параметр можно не указывать, но в случае необходимости он показывает нужно ли автоматически включать созданный объект после всех манипуляций (изначально включает).

Также возвращает компонент, как и в 1-й перегрузке, но уже можно с помощью 4-го параметра изменять будет ли объект на сцене включён или нет.

Пример использования:

```
ObjectBuilder.CreateNew(
    _testObject1Prefab,
    () => Instantiate(_testObject1Prefab),
    (testObject1Prefab) => testObject1Prefab.Construct("text"));
```

### Способы использования модуля:

### 1. Инициализация отдельных объектов

Большая часть всех объектов уже находится на сцене и просто передаются через сериализованные поля в группы.

Если нужно получить объект через поле, но и передать его в **CompositeRoot**, можно использовать сериализованные свойства:

```
[field: SerializeField] public int Property { get; private set; }
```

Метод **CreateEnvironment** не используется.

### 2. Конструирование сцены с нуля

Сцена изначально пустая за исключением объектов связанных с **CompositeRoot**, все объекты располагаются в виде префабов, после чего создаются на сцене уже на этапе выполнения. Можно пробрасывать все префабы через поля, но рекомендуется использовать **Prefab Control**.

Важно загрузка префабов для **Prefab Control** через метод **LoadPrefabs** уже встроена в **CompositeRoot**, делать это вручную не нужно

Метод **CreateEnvironment** используется для создания простых объектов не требующих инициализации.

Метод **Construct** используется для создания и инициализации сложных объектов.

Рекомендуется создавать все объекты через **ObjectBuilder**, для работы с **Prefab Control** у него имеется 3 дополнительные перегрузки метода **CreateNew**:

### 1-я перегрузка

```
GameObject CreateNew(PrefabName prefabName)
```

1-й параметр: значение из enum **PrefabName** в котором все зарегистрированные префабы (из **Prefab Control**)

Пример использования:

```
ObjectBuilder.CreateNew(PrefabName.TestObject2);
```

### 2-я перегрузка

```
T CreateNew<T>(PrefabName prefabName)
```

1-й параметр такой же как и в 1-й перегрузке

Нужно указать тип компонента, который оно вернёт с созданного объекта

Пример использования:

```
TestObject1 testObject1Prefab = ObjectBuilder.CreateNew<TestObject1>(PrefabName.GameObject1);
```

### 3-я перегрузка

```
T CreateNew<T>(PrefabName prefabName, Action<T> initializer, bool isActivateObject = true)
```

1-й параметр такой же как и в 1-й перегрузке

2-й параметр: эта функция, что используется для инициализации созданного объекта, записывается так:
`(<Имя параметра, как для метода>) => параметр.Construct(<какие-то аргументы>)`.

Нужно также указать тип компонента, который оно вернёт с созданного объекта

Пример использования:

```
ObjectBuilder.CreateNew<TestObject1>(
    PrefabName.GameObject1, (testObject1) => testObject1.Construct("text"));
```

### Пример использования с самой большой перегрузкой

```
TestObject1 testObject3Prefab = PrefabStorage.Get<TestObject1>(PrefabName.GameObject1);
ObjectBuilder.CreateNew(testObject3Prefab,
    () => Instantiate(testObject3Prefab),
    (testObject3) => testObject3.Construct("text"));
```

Тут используется перегрузка для обычных префабов и отдельно используется **Prefab Control** для получения такого префаба.

## Prefab Control

Это система для удобного контроля над префабами

### Правила

- Все префабы должны находиться в папке **Resources/Prefabs**
- Для проверки работоспособности в редакторе требуется после каждого добавления или удаления префабов обновлять информацию о них, это делается в верхней панели **Unity->Tools->Update Prefabs Info**

### Управление префабами

Подключаем пространство имён: **TanksArmageddon.Core.PrefabControl**

В котором есть enum **PrefabName**, в котором хранятся имена всех зарегистрированных префабов

Перед попыткой получить какой либо префаб, их необходимо сначала загрузить, это делается с помощью статического метода **LoadPrefabs** класса **PrefabStorage**.

После чего у этого же класса мы можем вызвать метод **Get** у которого есть 2 перегрузки:

### 1-я перегрузка

```
GameObject Get(PrefabName prefabName)
```

1-й параметр: значение из enum **PrefabName** в котором все зарегистрированные префабы

Пример использования:

```
GameObject testObjectPrefab = PrefabStorage.Get(PrefabName.GameObject1);
```

### 2-я перегрузка

```
T Get<T>(PrefabName prefabName)
```

1-й параметр такой же как и 1-й перегрузки

Метод возвращает компонент с выбранного префаба, который соответствует указанному типу.

Пример использования:

```
TestObject1 testObject3Prefab = PrefabStorage.Get<TestObject1>(PrefabName.GameObject1);
```

При билде игры данные о префабах обновятся автоматически

## Misc

Это модуль Core, который объединяет в себе полезные инструменты для разных частей Unity

Находится в пространстве имён **TanksArmageddon.Core**

### Список инструментов:

### 1. Vector3/Vector2 SetValues

Создаёт вектор на основе другого, заменяя определённые координаты

Пример использования:

```
Vector3 direction = new Vector3(5f, 3.2f, 7);
direction = direction.SetValues(y: 1.2f, z: 5f);
```

### 2. int/float IsInRange

Возвращает булево значение, показывающее, находится ли число в заданном диапазоне (включая границы)

Пример использования:

```
int number = 5;
if (number.IsInRange(-3, 8))
{
}
```
