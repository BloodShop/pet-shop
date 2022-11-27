# PetShop ASP.NET web Project using bootstrap lib

<h3>Asp.Net Core MVC web app using MSSQL and EF6</h3>
<p align="center" >Main catalog page where you can scroll and choose animel to explore and comment</p>
<p align="center">
  <img width="600"  src="https://user-images.githubusercontent.com/23366804/203003954-ec9ed699-a255-4281-ac26-0a370799e4a9.png">
</p>

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

  <h3>Bonus</h3>
  A global dark theme for the web. Dark Mode is an extension that helps you quickly turn the screen (browser) to dark at night time
  https://github.com/BloodShop/pet-shop/blob/bca1335742dcb8e3f82fede2aa8a21cb9e395060/wwwroot/js/theme-mode.js#L1-L33
  <i>path: ~\wwwroot\js\theme-mode.js</i>
<p align="center">
  <img width="600" src="https://user-images.githubusercontent.com/23366804/203011907-7f169f4e-4819-46f9-b773-d45802b58e51.png">
</p>
