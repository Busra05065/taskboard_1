import { taskApi, authApi } from './apiClient.js';

let currentUser = { isAuthenticated: false, username: null, role: null };


const loginForm = document.getElementById('loginForm');
const userProfile = document.getElementById('userProfile');
const loginUsername = document.getElementById('loginUsername');
const loginPassword = document.getElementById('loginPassword');
const displayUsername = document.getElementById('displayUsername');
const displayRole = document.getElementById('displayRole');
const logoutBtn = document.getElementById('logoutBtn');
const authError = document.getElementById('authError');


const taskForm = document.getElementById('taskForm');
const taskTitle = document.getElementById('taskTitle');
const taskPriority = document.getElementById('taskPriority');
const submitBtn = document.getElementById('submitBtn');
const formError = document.getElementById('formError');
const taskList = document.getElementById('taskList');
const emptyState = document.getElementById('emptyState');


async function checkAuth() {
    try {
        const user = await authApi.getCurrentUser();
        currentUser = user;
        updateAuthUI();
    } catch (_) {
        currentUser = { isAuthenticated: false };
        updateAuthUI();
    }
}

function updateAuthUI() {
    authError.textContent = '';
    if (currentUser.isAuthenticated) {
        loginForm.style.display = 'none';
        userProfile.style.display = 'flex';
        displayUsername.textContent = currentUser.username;
        displayRole.textContent = currentUser.role;
    } else {
        loginForm.style.display = 'flex';
        userProfile.style.display = 'none';
    }
    loadTasks();
}


loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    authError.textContent = '';
    try {
        await authApi.login(loginUsername.value, loginPassword.value);
        loginUsername.value = '';
        loginPassword.value = '';
        await checkAuth();
    } catch (err) {
        authError.textContent = err.message;
    }
});


logoutBtn.addEventListener('click', async () => {
    await authApi.logout();
    await checkAuth();
});


async function loadTasks() {
    try {
        formError.textContent = '';
        const tasks = await taskApi.getAll();
        renderTasks(tasks);
    } catch (err) {
        formError.textContent = err.message;
    }
}

function renderTasks(tasks) {
    taskList.innerHTML = '';
    if (!tasks || tasks.length === 0) {
        emptyState.style.display = 'block';
        return;
    }
    emptyState.style.display = 'none';

    tasks.forEach(task => {
        const item = document.createElement('div');
        item.className = 'task-card';

        
        const deleteButtonHtml = currentUser.role === 'Admin'
            ? `<button class="btn btn-danger delete-btn" data-id="${task.id}">Sil</button>`
            : '';

        item.innerHTML = `
            <div>
                <strong>${task.title}</strong>
                <span style="font-size: 12px; color: gray; margin-left: 6px;">[${task.priority}]</span>
            </div>
            <div>${deleteButtonHtml}</div>
        `;

        taskList.appendChild(item);
    });

    
    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', () => deleteTask(btn.dataset.id, btn));
    });
}


taskForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    formError.textContent = '';
    submitBtn.disabled = true;

    try {
        await taskApi.create({
            title: taskTitle.value,
            priority: taskPriority.value
        });
        taskTitle.value = '';
        await loadTasks();
    } catch (err) {
        formError.textContent = err.message;
    } finally {
        submitBtn.disabled = false;
    }
});


async function deleteTask(id, btnElement) {
    if (!confirm('Bu görevi silmek istediğinize emin misiniz?')) return;

    if (btnElement) btnElement.disabled = true;

    try {
        await taskApi.delete(id);
        await loadTasks();
    } catch (err) {
        
        alert(err.message);
        if (btnElement) btnElement.disabled = false;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    checkAuth();
});