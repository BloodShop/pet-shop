document.addEventListener('DOMContentLoaded', () => {
    const body = document.querySelector('body');
    const inputs = document.querySelectorAll('input');
    const containers = document.querySelectorAll('.container');
    const dropdown = document.querySelector('.dropdown-menu');
    const textAreas = document.querySelectorAll('textarea.form-control');
    const problem = document.getElementById('Problem');
    const ulss = document.querySelectorAll('li.list-group-item.d-flex.justify-content-between.align-items-start');
    const navBar = document.querySelector('body>nav');
    const aas = document.querySelectorAll('body div.container-fluid a, form>button.nav-link.btn.btn-link.py-0').forEach(a => {
        a.classList.toggle('text-light');
    });
    const toggle = document.getElementById('toggle');
    toggle.onclick = function () {
        toggle.classList.toggle('active');
        body.classList.toggle('active');
        containers.forEach(e => {
            e.classList.toggle('active');
            //let children = e.children;
            //for (let i = 0; i < children.length; i++) {
            //    children[i].classList.toggle('active');
        });
        dropdown.classList.toggle('bg-dark');
        dropdown.querySelectorAll('a').forEach(a => {
            a.classList.toggle('text-dark');
        });
        navBar.classList.toggle('bg-opacity-25');
        inputs.forEach(i => {
            i.classList.toggle('bg-black');
        });
        ulss.forEach(li => {
            li.classList.toggle('bg-dark');
        });
        textAreas.forEach(ta => {
            ta.classList.toggle('bg-dark');
        });
        problem.classList.toggle('bg-dark');
    };
});