<script setup>
    import { ref } from 'vue';
    const props = defineProps({
        project: {
            type: Object,
            required: true,
            default: () => ({
                name: '',
                description: '',
                startDate: '',
                endDate: '',
                status: ''
            })
        }
    });

    // Computed or Reactive Variables
    const formattedStartDate = ref(new Date(props.project.startDate).toLocaleDateString());
    const formattedEndDate = ref(new Date(props.project.endDate).toLocaleDateString());
</script>

<template>
    <div class="project-overview">
        <h2 class="project-name">{{ project.name }}</h2>
        <p class="project-description">{{ project.description }}</p>

        <div class="project-details">
            <p><strong>Start Date:</strong> {{ formattedStartDate }}</p>
            <p><strong>End Date:</strong> {{ formattedEndDate }}</p>
            <p><strong>Status:</strong> {{ project.status }}</p>
        </div>
    </div>
</template>

<style scoped>
    .project-overview {
        padding: 16px;
        border: 1px solid #ddd;
        border-radius: 8px;
        background-color: #f9f9f9;
    }

    .project-name {
        font-size: 24px;
        margin-bottom: 8px;
    }

    .project-description {
        font-size: 16px;
        color: #666;
        margin-bottom: 16px;
    }

    .project-details p {
        margin: 4px 0;
    }

    .project-details strong {
        color: #333;
    }
</style>
