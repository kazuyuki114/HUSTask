<script setup>
    import { ref, onMounted } from 'vue';
    import { useRoute } from 'vue-router';
    import axios from 'axios';
    import ProjectOverviewSection from '@/components/ProjectOverviewSection.vue';
    import TableOfMembers from '@/components/TableOfMembers.vue';

    // State
    const project = ref(null);
    const members = ref([]);
    const route = useRoute();
    const isLeader = ref(false);
    // Extract projectId from route params and convert to a number
    const currentUserId = Number(localStorage.getItem('userId'));
    const projectId = ref(Number(route.params.projectId));

    // Fetch data on mount
    const fetchProjectDetails = async () => {
        try {
            const response = await axios.get(`http://localhost:5059/api/project/${projectId.value}`);
            project.value = response.data;
        } catch (error) {
            console.error('Error fetching project details:', error);
        }
    };

    const fetchMembers = async () => {
        try {
            // Make the API call
            const membersResponse = await axios.get(`http://localhost:5059/api/ProjectMember/project/${projectId.value}`);
            console.log(membersResponse.data);

            // Check if the current user is the project leader
            const currentUser = membersResponse.data.find(member => member.userId === currentUserId);
            isLeader.value = currentUser?.role === 'Leader';

            // Assign the response data to the reactive members variable
            members.value = membersResponse.data.map(member => ({
                userId: member.userId,
                userName: member.user?.name || 'Unknown', // Safely access user properties
                userEmail: member.user?.email || 'Unknown',
                role: member.role || 'N/A' // Default role if not provided
            }));

            // Log the processed members
            console.log(members.value);
        } catch (error) {
            console.error('Error fetching members:', error.message);
        }
    };

    onMounted(async () => {
        await fetchProjectDetails();
        await fetchMembers();
    });
</script>


<template>
    <div class="project-detail-view container mx-auto px-4 bg-gray-50">
        <!-- Project Overview Section -->
        <div class="project-overview bg-white shadow rounded-lg p-6 mb-4 w-full">
            <h2 class="section-header">Project Overview</h2>
            <ProjectOverviewSection v-if="project" :project="project" />
        </div>

        <!-- Members Table Section -->
        <div class="members-section bg-white shadow rounded-lg p-6 w-full">
            <h2 class="section-header">Project Members</h2>
            <TableOfMembers v-if="members.length" :members="members" :projectId="projectId" :isLeader="isLeader" />
        </div>
    </div>
</template>

<style scoped>
    .project-detail-view {
        padding: 16px;
        background-color: #f8f9fa;
    }

    .section-header {
        margin-bottom: 12px;
        font-size: 1.5rem;
        font-weight: bold;
        color: #333;
        border-bottom: 2px solid #007bff;
        padding-bottom: 4px;
    }

    .project-overview,
    .members-section {
        margin-bottom: 0;
        /* Remove unnecessary space */
        max-width: 100%;
        /* Ensure full width usage */
    }
</style>
