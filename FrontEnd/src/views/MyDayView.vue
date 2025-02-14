<template>
    <div class="kanban-board flex space-x-8 py-4">
        <!-- Completed Today Section -->
        <div class="kanban-column w-1/2 bg-gray-100 rounded-lg p-6">
            <div class="flex items-center justify-between mb-4">
                <h3 class="text-xl font-semibold text-gray-700">Completed Today</h3>
                <!-- Add Task Button -->
                <button @click="openAddModal('completed')"
                    class="bg-blue-500 text-white rounded-full w-7 h-7 flex items-center justify-center hover:bg-blue-600 focus:outline-none shadow-lg transform transition-all duration-300 ease-in-out hover:scale-110">
                    <span class="text-xl font-bold">+</span>
                </button>
            </div>

            <div v-if="completedTasks.length === 0" class="text-gray-500 text-center mt-4">No completed tasks</div>

            <div v-else>
                <div v-for="task in completedTasks" :key="task.id" class="mb-4">
                    <TaskComponent :task="task" @remove-task="removeTask" @edit-task="openEditModal" />
                </div>
            </div>
        </div>

        <!-- In Progress Today Section -->
        <div class="kanban-column w-1/2 bg-gray-100 rounded-lg p-6">
            <div class="flex items-center justify-between mb-4">
                <h3 class="text-xl font-semibold text-gray-700">In Progress Today</h3>
                <!-- Add Task Button -->
                <button @click="openAddModal('completed')"
                    class="bg-blue-500 text-white rounded-full w-7 h-7 flex items-center justify-center hover:bg-blue-600 focus:outline-none shadow-lg transform transition-all duration-300 ease-in-out hover:scale-110">
                    <span class="text-xl font-bold">+</span>
                </button>
            </div>

            <div v-if="inProgressTasks.length === 0" class="text-gray-500 text-center mt-4">No tasks in progress</div>

            <div v-else>
                <div v-for="task in inProgressTasks" :key="task.id" class="mb-4">
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
    const completedTasks = ref([]);
    const inProgressTasks = ref([]);

    const isAddModalVisible = ref(false);
    const isEditModalVisible = ref(false);
    const currentTask = ref(null);
    const currentTaskType = ref(''); // Add this line to define currentTaskType

    const fetchTasks = async () => {
        try {
            const response = await axios.get(`http://localhost:5059/api/TaskAssignment/user/${userId}`);
            tasks.value = Array.isArray(response.data) ? response.data : [];
            categorizeTasks();
        } catch (error) {
            console.error('Error fetching tasks:', error.message);
            tasks.value = [];
        }
    };

    const categorizeTasks = () => {
        const today = dayjs().startOf('day'); // Get today at midnight

        completedTasks.value = [];
        inProgressTasks.value = [];

        tasks.value.forEach(task => {
            const taskDate = dayjs(task.dueDate).startOf('day'); // Normalize task date to midnight

            if (taskDate.isSame(today)) {
                if (task.status === 'completed') {
                    completedTasks.value.push(task);
                } else if (task.status === 'pending') {
                    inProgressTasks.value.push(task);
                }
            }
        });
    };

    const openAddModal = (taskType) => {
        currentTaskType.value = taskType; // This should now work correctly
        isAddModalVisible.value = true;
    };

    const openEditModal = (task) => {
        currentTask.value = task;
        isEditModalVisible.value = true;
    };

    const addNewTask = (newTask) => {
        newTask.userId = Number(userId);
        tasks.value.push(newTask);
        categorizeTasks();
        isAddModalVisible.value = false;
    };

    const updateTask = (updatedTask) => {
        const index = tasks.value.findIndex(task => task.id === updatedTask.id);
        if (index !== -1) {
            tasks.value[index] = updatedTask;
            categorizeTasks();
        }
        isEditModalVisible.value = false;
    };

    const removeTask = (taskId) => {
        tasks.value = tasks.value.filter(task => task.id !== taskId);
        categorizeTasks();
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
    .kanban-board {
        display: flex;
        gap: 2rem;
        padding: 16px;
    }

    .kanban-column {
        flex: 1;
        max-width: 50%;
    }

    .kanban-column h3 {
        margin-bottom: 16px;
    }

    button {
        display: flex;
        justify-content: center;
        align-items: center;
    }
</style>