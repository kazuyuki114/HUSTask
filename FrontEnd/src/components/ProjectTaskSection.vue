<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import TaskComponent from '@/components/ProjectTaskCard.vue';
    import CommentCard from './CommentCard.vue';
    import AddProjectTaskModal from '@/components/AddProjectTaskModal.vue';
    import EditProjectTaskModal from '@/components/EditProjectTaskModal.vue'; // Import the edit modal
    import dayjs from 'dayjs';
    import { useRoute } from 'vue-router';
    import { useToast } from 'vue-toastification';

    // Get userId from route params
    const route = useRoute();
    const userId = Number(localStorage.getItem('userId'));
    const projectId = Number(route.params.projectId);
    const memberId = Number(route.params.memberId);
    const toast = useToast();
    const currentUser = ref();
    // Task data (initialized empty, will be fetched from API)
    const tasks = ref([]);
    const overdueTasks = ref([]);
    const upcomingTasks = ref([]);
    const isLeader = ref(false);
    // Modal visibility and current task details
    const isAddModalVisible = ref(false);
    const isEditModalVisible = ref(false);
    const currentTaskType = ref('');
    const currentTask = ref(null); // Task being edited

    // Comments for the selected task
    const comments = ref([]);
    const selectedTaskId = ref(null);
    const newCommentContent = ref("");
    const showAddCommentSection = ref(false);  // Whether the add comment section is visible


    // Fetech current User info
    const fetchCurrentUser = async () => {
        try {
            // Make an API request to fetch the current user's info
            const response = await axios.get(`http://localhost:5059/api/users/${userId}`);  // Change the URL as needed
            // Assuming the response contains the user data
            currentUser.value = response.data;
            console.log("Current user fetched:", currentUser.value);  // For debugging

        } catch (error) {
            console.error("Error fetching current user:", error);
        }
    };

    // Check leader function
    const checkLeader = async () => {
        try {
            // Check if the user is the leader of the project
            const response = await axios.get(`http://localhost:5059/api/ProjectMember/project/${projectId}/user/${userId}`);
            console.log(response.data);
            // If the user is the leader of the project, proceed
            if (response.data.role === 'Leader') {
                isLeader.value = true;
            }
            console.log(isLeader.value);
        } catch (error) {
            console.error(error);
        }
    };

    // Fetch tasks from the updated API endpoint
    const fetchTasks = async () => {
        try {
            const response = await axios.get(
                `http://localhost:5059/api/ProjectAssignment/user/${memberId}/project/${projectId}`
            );

            // Map the response data to extract relevant task information
            tasks.value = Array.isArray(response.data)
                ? response.data.map((assignment) => ({
                    id: assignment.projectTask?.id || null,
                    title: assignment.projectTask?.title || '',
                    description: assignment.projectTask?.description || '',
                    status: assignment.projectTask?.status || 'pending',
                    dueDate: assignment.projectTask?.dueDate || null,
                    priorityId: assignment.projectTask?.priorityId || null,
                    assignedTime: assignment.assignedTime || null,
                    projectId: assignment.projectId,
                    userId: assignment.userId,
                }))
                : [];

            console.log('Mapped Tasks:', tasks.value);
            updateTaskLists();
        } catch (error) {
            console.error('Error fetching tasks:', error.message);
            tasks.value = [];
        }
    };

    // Categorize tasks based on due date
    const updateTaskLists = () => {
        // Overdue tasks: Status is 'completed' and due date is before today
        overdueTasks.value = tasks.value.filter(task =>
            task.status === 'completed' || task.status === 'overdue' || dayjs(task.dueDate).isBefore(dayjs(), 'day')
        );

        // Upcoming tasks: Tasks with due date after today
        upcomingTasks.value = tasks.value.filter(task =>
            task.status === 'pending' && (dayjs(task.dueDate).isSame(dayjs(), 'day') || dayjs(task.dueDate).isAfter(dayjs(), 'day'))
        );
    };

    // Open modals
    const openAddModal = (taskType) => {
        currentTaskType.value = taskType;
        isAddModalVisible.value = true;
    };

    const openEditModal = (task) => {
        currentTask.value = task;
        isEditModalVisible.value = true;
    };

    // Update task after editing
    const updateTask = (updatedTask) => {
        const index = tasks.value.findIndex(task => task.id === updatedTask.id);
        if (index !== -1) {
            tasks.value[index] = updatedTask;
            updateTaskLists();
            toast.success('Update the task successfully');
        }
        isEditModalVisible.value = false;
    };

    // Add new task
    const addNewTask = (newTask) => {
        newTask.userId = Number(userId);
        tasks.value.push(newTask);
        updateTaskLists();
        isAddModalVisible.value = false;
        toast.success('Add a new task successfully!!');
    };

    // Close modals
    const closeAddModal = () => {
        isAddModalVisible.value = false;
    };

    const closeEditModal = () => {
        isEditModalVisible.value = false;
    };

    // Remove task
    const removeTask = (taskId) => {
        tasks.value = tasks.value.filter(task => task.id !== taskId);
        updateTaskLists();
        toast.success('Remove the task successfully!!');
    };

    // Function to toggle visibility of the add comment section
    const toggleAddCommentSection = () => {
        showAddCommentSection.value = !showAddCommentSection.value;
    };

    // View Comment
    const viewComment = async (taskId) => {
        try {
            console.log(`Fetching comments for Task ID: ${taskId}`);
            const response = await axios.get(`http://localhost:5059/api/Comment/task/${Number(taskId)}`);
            comments.value = response.data;

            selectedTaskId.value = taskId;

            console.log("Fetched Comments:", comments.value);
            if (comments.value.length === 0) {
                console.log(`No comments found for Task ID ${taskId}.`);
            } else {
                console.log(`Fetched ${comments.value.length} comments for Task ID ${taskId}.`);
            }
        } catch (error) {
            console.error("Error fetching comments:", error);
            comments.value = [];
            selectedTaskId.value = taskId;
        }
    };

    const addComment = async () => {
        if (!newCommentContent.value.trim()) {
            console.error("Comment content cannot be empty.");
            return;
        }
        try {
            const response = await axios.post("http://localhost:5059/api/Comment", {
                content: newCommentContent.value,
                taskId: selectedTaskId.value,
                userId: userId,
            });

            // Assuming the response includes the comment and the commentedUser information
            const newComment = response.data;

            // Push the new comment to the comments array
            comments.value.push({
                id: newComment.id,
                content: newComment.content,
                taskId: newComment.taskId,
                updated_at: newComment.updated_at,
                commentedUser: currentUser || { name: "Unknown User", email: "No Email" }
            });

            newCommentContent.value = ""; // Clear the input field
            showAddCommentSection.value = false;  // Hide the add comment section

        } catch (error) {
            console.error("Error adding comment:", error);
        }
    };
    const deleteComment = async (id) => {
        console.log(id);
        try {
            await axios.delete(`http://localhost:5059/api/comment/${id}`)
            // Remove the deleted comment from the local list
            comments.value = comments.value.filter(comment => comment.id !== id);
        } catch (error) {
            console.error(`Error deleting comment with ID ${id}:`, error);
        }

    };

    // Fetch tasks when the component is mounted
    onMounted(() => {
        fetchTasks();
        checkLeader();
        fetchCurrentUser();
    });
</script>

<template>
    <div class="p-6 bg-gray-100 min-h-[50vh] flex w-full">
        <h1 class="sr-only">Kanban Board</h1>
        <div class="flex w-full space-x-4">
            <!-- Overdue Tasks Column -->
            <div class="flex-1 bg-white shadow-md rounded-lg p-4">
                <h2 class="text-lg font-semibold mb-2 text-red-500">Overdue Tasks</h2>
                <div class="space-y-2">
                    <TaskComponent v-for="task in overdueTasks" :key="task.id" :task="task" :isLeader="isLeader"
                        @remove-task="removeTask" @edit-task="openEditModal" @view-comment="viewComment" />
                </div>
            </div>

            <!-- Upcoming Tasks Column -->
            <div class="flex-1 bg-white shadow-md rounded-lg p-4">
                <div class="flex justify-between items-center">
                    <h2 class="text-lg font-semibold mb-2 text-green-500">Upcoming Tasks</h2>
                    <button v-if="isLeader" @click="openAddModal('upcoming')"
                        class="bg-green-500 text-white rounded-full w-7 h-7 flex items-center justify-center hover:bg-green-600 focus:outline-none shadow-lg transform transition-all duration-300 ease-in-out hover:scale-110">
                        <span class="text-xl font-bold">+</span>
                    </button>
                </div>
                <div class="space-y-2">
                    <TaskComponent v-for="task in upcomingTasks" :key="task.id" :task="task" :isLeader="isLeader"
                        @remove-task="removeTask" @edit-task="openEditModal" @view-comment="viewComment" />
                </div>
            </div>

            <!-- Comment Column -->
            <div class="flex-1 bg-white shadow-md rounded-lg p-4">
                <h2 class="text-lg font-semibold mb-2 text-blue-500">Comments</h2>

                <div v-if="selectedTaskId" class="space-y-4">
                    <!-- Comments List -->
                    <div class="space-y-2">
                        <!-- Task Selected but No Comments -->
                        <div v-if="comments.length === 0" class="text-gray-500">
                            No comments available for this task.
                        </div>
                        <!-- Task Selected with Comments -->
                        <CommentCard v-for="comment in comments" :key="comment.id" :comment="comment"
                            :current-user="userId" @delete-comment="deleteComment(comment.id)" />
                    </div>

                    <!-- Add Comment Section (Initially hidden) -->
                    <div v-if="showAddCommentSection" class="space-y-2 mt-4">
                        <textarea v-model="newCommentContent" placeholder="Write your comment..."
                            class="w-full border rounded-md p-2 focus:outline-none focus:ring focus:ring-blue-300"></textarea>

                        <!-- Buttons Container -->
                        <div class="flex justify-between gap-4">
                            <!-- Add Comment Button -->
                            <button @click="addComment"
                                class="bg-blue-500 text-white px-6 py-3 rounded-md hover:bg-blue-600 w-full md:w-auto">
                                Add Comment
                            </button>

                            <!-- Cancel Button -->
                            <button @click="toggleAddCommentSection"
                                class="bg-gray-500 text-white px-6 py-3 rounded-md hover:bg-gray-600 w-full md:w-auto">
                                Cancel
                            </button>
                        </div>
                    </div>


                    <!-- "+" Button to Toggle Add Comment Section -->
                    <div v-else class="flex justify-center mt-4">
                        <button @click="toggleAddCommentSection"
                            class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600">
                            +
                        </button>
                    </div>

                </div>

                <!-- No Task Selected -->
                <div v-else class="text-gray-500">
                    Select a task to view comments.
                </div>
            </div>

        </div>

        <!-- Add Task Modal -->
        <AddProjectTaskModal v-if="isAddModalVisible" :userId="memberId" :projectId="projectId" @add-task="addNewTask"
            @close-modal="closeAddModal" />

        <!-- Edit Task Modal -->
        <EditProjectTaskModal v-if="isEditModalVisible" :taskData="currentTask" @update-task="updateTask"
            @close-modal="closeEditModal" />
    </div>
</template>
