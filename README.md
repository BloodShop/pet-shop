# PetShop ASP.NET Core web application Project 

<h3>Asp.Net Core MVC web appplication using mssql, ef6, docker, jquery, sass, CORS, restAPI, socket, hubs & signalR, vanilla js, bootstrap library</h3>
<p align="center" >Main catalog page where you can scroll and choose animel to explore and comment</p>
<p align="center">
  <img width="600"  src="https://user-images.githubusercontent.com/23366804/203003954-ec9ed699-a255-4281-ac26-0a370799e4a9.png">
</p>

<h3>Docker instuctions</h3>
pulling images to your local device:

```javascript
  $ docker pull bloodshop/petshopapp:1.0  # https://hub.docker.com/repository/docker/bloodshop/petshopapp
  $ docker pull bloodshop/petshopdb:1.0  # https://hub.docker.com/repository/docker/bloodshop/petshopdb
  $ docker-compose up -d
```

<h2>docker-compose.yml</h2>

```python
version: '3.3'

services:
  db:
    image: bloodshop/petshopdb:1.0
    restart: always

  app:
    depends_on:
      - db
    image: bloodshop/petshopapp:1.0
    ports: 
        - "3000:80" 
        - "3001:433"
    networks: 
       - db-bridge
    restart: always
```

<h2>Clear Dependency Injection</h2>
Here is an implementation of adding all the additional services
https://github.com/BloodShop/pet-shop/blob/bcf9df3bc1e9b9ca9d170aeadf3c22c8588f338b/Program.cs#L15-L17

```javascript
public class InfrastuctureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IRepository, PetRepository>();
        string connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddControllersWithViews().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        services.AddDbContext<ICallCenterContext, PetDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
    }
}

public interface IServiceInstaller
{
    void Install(IServiceCollection services,IConfiguration configuration);
}
```

<h3>What's in this project</h3>
This is a sample which shows most of the common features of ASP.NET Identity. For more information on it, please visit http://asp.net/identity 
<ul>
<li>
<b>ViewModels & models</b><br/>
      <b>Models - pure models which are used in our database and repositories</b>
        <ul>
            <li>Animal</li>
            <li>Category</li>
            <li>Comment</li>
        </ul>
      <b>ViewModels - added value for implementing a model and binding it with form in-order to create an instance with limitations</b>
         <ul>
            <li>AddAnimalViewModel</li>
            <li>AddCategoryViewModel</li>
            <li>AddCommentViewModel</li>
            <li>EditAnimalViewModel</li>
            <li>LoginViewModel</li>
            <li>ManageUsersViewModel</li>
            <li>RegisterViewModel</li>
            <li>SearchAnimalViewModel</li>
        </ul>
        <p>
     <i>examlpe viewmodel binded with form/view</i>
        <p>
     <br/>
     <b>Viewmodel</b>
     https://github.com/BloodShop/pet-shop/blob/bca1335742dcb8e3f82fede2aa8a21cb9e395060/ViewModels/SearchAnimalViewModel.cs#L5-L11
     <b>View</b><br/>
<p>
     
```javascript
@{
  var searchModel = new SearchAnimalViewModel();
}
```

</p>
     https://github.com/BloodShop/pet-shop/blob/bca1335742dcb8e3f82fede2aa8a21cb9e395060/Views/Shared/_Layout.cshtml#L87-L91
</li>
<li>
    <b>Initialize ASP.NET Identity</b>
        You can initialize ASP.NET Identity when the application starts. Since ASP.NET Identity is Entity Framework based in this sample,
        you can create DatabaseInitializer which is configured to get called each time the app starts.
        <strong>Please look in Program.cs</strong>
        https://github.com/BloodShop/pet-shop/blob/bca1335742dcb8e3f82fede2aa8a21cb9e395060/Program.cs#L16-L29
        This code shows the following
        <ul>
            <li>Create user</li>
            <li>Create user with password</li>
            <li>Create Roles</li>
            <li>Add Users to Roles</li>
        </ul>
</li>
<li>
       <b>Validation</b>
             When you create a User using a username or password, the Identity system performs validation on the username and password, and the passwords are hashed before they are
                stored in the database. You can customize the validation by changing some of the properties of the validators such as Turn alphanumeric on/off, set minimum password length
                or you can write your own custom validators and register them with the Administrator. You can use the same approach for UserManager and RoleManager.
                <ul>
                    <li>Look at Controllers\AccountController.cs Default Action on how to tweak the default settings for the Validators</li>
                    <li>Look at Models\DataAnnotations\ValidateFileAttribute.cs to see how you can implement the different validators</li>
                    https://github.com/BloodShop/pet-shop/blob/bca1335742dcb8e3f82fede2aa8a21cb9e395060/Models/DataAnnotations/ValidateFileAttribute.cs#L5-L38
                    <li>Look at Controllers\AccountController.cs Cutomize Action on how you can use the custom validators with the Managers</li>
                </ul>

</li>
<li>
    <b>Register a user, Login</b>
    Click Register and see the code in AccountController.cs and Register Action.
        Click Login and see the code in AccountController.cs and Login Action.
</li>
<li>
    <b>Basic Role Management</b>
    Do Create, Update, List and Delete Roles.
        Only Users In Role Admin can access this page. This uses the [Authorize] on the controller.
</li>
<li>
    <b>Basic User Management</b>
        Do Create, Update, List and Delete Users.
        Assign a Role to a User.
        Only Users In Role Admin can access this page. This uses the [Authorize] on the controller.
</li>

  <h3>SignalR</h3>
  https://github.com/BloodShop/pet-shop/blob/7948044ae63007023e9f320aa13194d46922b412/Hubs/CallCenterHub.cs#L6-L19
  ASP.NET Core SignalR is an open-source library that simplifies adding real-time web functionality to apps. Real-time web functionality enables server-side code to push content to clients instantly. </br>
  !!Importent!! -- Providing the CallsController that Hub implementing the `ICallCenterHub` interface inorder to get the dependencies (Dependency Injection).
  
```javascript
$(() => {
    LoadCallsData();

    let $theWarning = $("#theWarning");
    $theWarning.hide();

    var connection = new signalR.HubConnectionBuilder().withUrl("/callcenter").build();
    connection.start()
        .then(() => connection.invoke("JoinCallCenters"))
        .catch(err => console.error(err.toString()));

    connection.on("NewCallReceivedAsync", () => LoadCallsData());
    connection.on("CallDeletedAsync", () => LoadCallsData());
    connection.on("CallEditedAsync", () => LoadCallsData());

    function LoadCallsData() {
        var tr = '';
        $.ajax({
            url: '/Calls/GetCalls',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td>${v.Name}</td>
                        <td>${v.Email}</td>
                        <td>${moment(v.CallTime).format("llll")}</td>
                        <td>
                            <a href="../Calls/Details?id=${v.Id}" class="btn btn-sm btn-success deatils-button" data-id="${v.Id}">Details</a>
                            <a href="../Calls/Edit?id=${v.Id}" class="btn btn-sm btn-danger edit-button" data-id="${v.Id}">Edit</a>
                            <a href="../Calls/Delete?id=${v.Id}" class="btn btn-sm btn-warning delete-button" data-id="${v.Id}">Delete</a>
                        </td>
                    </tr>`;
                })
                $("#logBody").html(tr);
            },
            error: (error) => {
                $theWarning.text("Failed to get calls...");
                $theWarning.show();
                console.log(error)
            }
        });
    }
});
```

  <h3>Bonus</h3>
  A global dark theme for the web. Dark Mode is an extension that helps you quickly turn the screen (browser) to dark at night time
  https://github.com/BloodShop/pet-shop/blob/bca1335742dcb8e3f82fede2aa8a21cb9e395060/wwwroot/js/theme-mode.js#L1-L33
  <i>path: ~\wwwroot\js\theme-mode.js</i>
<p align="center">
  <img width="600" src="https://user-images.githubusercontent.com/23366804/203011907-7f169f4e-4819-46f9-b773-d45802b58e51.png">
</p>
