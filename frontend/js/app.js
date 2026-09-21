
const form = document.querySelector('#task-form');
const titleInput = document.querySelector('#title');
const prioritySelect = document.querySelector('#priority');
const tableBody = document.querySelector('#task-table-body');
const emptyState = document.querySelector('#empty-state');


const totalTasksEl = document.querySelector('#total-tasks');
const openTasksEl = document.querySelector('#open-tasks');


form.addEventListener('submit', function (event) {
   
    event.preventDefault();

   
    const title = titleInput.value.trim();
    const priority = prioritySelect.value;

    
    if (!title) {
        return;
    }

    
    let priorityBadge = '';
    let isHighPriority = false;

    if (priority === 'high') {
        priorityBadge = '<span class="badge badge-high">Yüksek</span>';
        isHighPriority = true;
    } else if (priority === 'normal') {
        priorityBadge = '<span class="badge badge-normal">Normal</span>';
    } else {
        priorityBadge = '<span class="badge badge-low">Düşük</span>';
    }

   
    const today = new Date().toISOString().split('T')[0];

    
    const newRowHTML = `
        <tr class="${isHighPriority ? 'priority-high-row' : ''}">
            <td>${isHighPriority ? `<strong>${title}</strong>` : title}</td>
            <td>${priorityBadge}</td>
            <td><span class="status status-open">Açık</span></td>
            <td>${today}</td>
        </tr>
    `;

    
    tableBody.insertAdjacentHTML('afterbegin', newRowHTML);

    
    if (emptyState) {
        emptyState.hidden = true;
    }

    
    let currentTotal = parseInt(totalTasksEl.textContent, 10) || 0;
    let currentOpen = parseInt(openTasksEl.textContent, 10) || 0;
    totalTasksEl.textContent = currentTotal + 1;
    openTasksEl.textContent = currentOpen + 1;

    
    form.reset();
    titleInput.focus();
});