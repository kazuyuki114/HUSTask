<template>
    <div class="kanban-board-container">
        <div class="kanban-board flex space-x-4 py-4">
            <!-- Loop through the 5 columns (for each day) -->
            <div v-for="(tasksForDay, index) in taskByDay" :key="index"
                class="kanban-column bg-gray-100 rounded-lg p-4">
                <!-- Container for Title and Button -->
                <div class="flex items-center justify-between mb-4">
                    <h3 class="text-xl font-semibold text-gray-700">
                        {{ dayjs().add(index, 'days').format('MMMM D') }}
                    </h3>
                    <!-- Add button to the right of the title -->
                    <button @click="openAddModal('day_' + index)"
                        class="bg-blue-500 text-white rounded-full w-7 h-7 flex items-center justify-center hover:bg-blue-600 focus:outline-none shadow-lg transform transition-all duration-300 ease-in-out hover:scale-110">
                        +
                    </button>
                </div>

                <div v-if="tasksForDay.length === 0" class="text-gray-500 text-center mt-4">No tasks</div>

                <div v-else>
                    <div v-for="task in tasksForDay" :key="task.id" class="mb-4">
                        <TaskComponent :task="task" @remove-task="removeTask" @edit-task="openEditModal" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Add Task Modal -->
        <AddTaskModal v-if="isAddModalVisible" :userId="userId" @add-task="addNewTask" @close-modal="closeAddModal" />

        <!-- Edit Task Modal -->
        <EditTaskModal v-if="isEditModalVisible" :taskData="currentTask" @update-task="updateTask"
            @close-modal="closeEditModal" />
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import TaskComponent from '@/components/Task.vue';
    import AddTaskModal from '@/components/AddTaskModal.vue';
    import EditTaskModal from '@/components/EditTaskModal.vue';
    import dayjs from 'dayjs';
    import { useRoute } from 'vue-router';

    const route = useRoute();
    const userId = localStorage.getItem('userId');

    const tasks = ref([]);
    const taskByDay = ref([]); // Array to store tasks categorized by the days (5 columns)

    const isAddModalVisible = ref(false);
    const isEditModalVisible = ref(false);
    const currentTask = ref(null);
    const currentTaskType = ref('');

    const fetchTasks = async () => {
        try {
            const response = await axios.get(`http://localhost:5059/api/TaskAssignment/user/${userId}`);
            tasks.value = Array.isArray(response.data) ? response.data : [];
            categorizeTasksByDay();
        } catch (error) {
            console.error('Error fetching tasks:', error.message);
            tasks.value = [];
        }
    };
    const categorizeTasksByDay = () => {
        const today = dayjs().startOf('day'); // Get today at midnight

        taskByDay.value = Array.from({ length: 5 }, () => []); // Only 5 columns

        tasks.value.forEach(task => {
            if (task.status === 'pending') {
                const taskDate = dayjs(task.dueDate).startOf('day'); // Normalize the task's due date to midnight
                const diff = taskDate.diff(today, 'days'); // Difference in days

                if (diff >= 0 && diff <= 4) { // Adjusted for 5 days instead of 7
                    taskByDay.value[diff].push(task); // Assign task to the corresponding day
                }
            }
        });
    };

    const openAddModal = (taskType) => {
        currentTaskType.value = taskType;
        isAddModalVisible.value = true;
    };

    const openEditModal = (task) => {
        currentTask.value = task;
        isEditModalVisible.value = true;
    };

    const addNewTask = (newTask) => {
        newTask.userId = Number(userId);
        tasks.value.push(newTask);
        categorizeTasksByDay();
        isAddModalVisible.value = false;
    };

    const updateTask = (updatedTask) => {
        const index = tasks.value.findIndex(task => task.id === updatedTask.id);
        if (index !== -1) {
            tasks.value[index] = updatedTask;
            categorizeTasksByDay();
        }
        isEditModalVisible.value = false;
    };

    const removeTask = (taskId) => {
        tasks.value = tasks.value.filter(task => task.id !== taskId);
        categorizeTasksByDay();
    };

    const closeAddModal = () => {
        isAddModalVisible.value = false;
    };

    const closeEditModal = () => {
        isEditModalVisible.value = false;
    };

    onMounted(fetchTasks);
</script>

<style scoped>

    /* Full height container for Kanban board */
    .kanban-board-container {
        display: flex;
        flex-direction: column;
        /* Ensures that the columns are stacked vertically */
        height: 100vh;
        /* Full height of the screen */
        overflow-y: auto;
        /* Allow scrolling vertically */
        padding-bottom: 20px;
        /* Ensure space for scrollbar */
    }

    /* Container for task columns */
    .kanban-board {
        display: flex;
        gap: 1rem;
        padding: 16px;
        flex-grow: 1;
        /* Allows this container to expand and fill available space */
    }

    /* Style for individual task columns */
    .kanban-column {
        min-width: calc(100% / 5);
        /* 5 columns across the screen */
        max-width: calc(100% / 5);
        flex-shrink: 0;
    }

    .kanban-column h3 {
        margin-bottom: 16px;
    }

    /* Button styling for add task button */
    button {
        display: flex;
        justify-content: center;
        align-items: center;
    }

    /* Optional: Media queries for responsive layout */
    @media (max-width: 1024px) {
        .kanban-column {
            min-width: 20rem;
            max-width: 20rem;
        }
    }

    @media (max-width: 640px) {
        .kanban-column {
            min-width: 14rem;
            max-width: 14rem;
        }
    }
</style>
