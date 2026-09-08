package com.demo.controller;

import com.demo.model.FeatureImplementation;
import com.demo.repository.FeatureImplementationRepository;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import java.util.Optional;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

@WebMvcTest(FeatureImplementationController.class)
public class FeatureImplementationControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private FeatureImplementationRepository repository;

    @Test
    public void testGetById_Found() throws Exception {
        FeatureImplementation item = new FeatureImplementation("Sample FeatureImplementation", "Sample Description");
        item.setId(1L);
        Mockito.when(repository.findById(1L)).thenReturn(Optional.of(item));

        mockMvc.perform(get("/api/featureimplementations/1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.name").value("Sample FeatureImplementation"));
    }

    @Test
    public void testGetById_NotFound() throws Exception {
        Mockito.when(repository.findById(99L)).thenReturn(Optional.empty());

        mockMvc.perform(get("/api/featureimplementations/99"))
                .andExpect(status().isNotFound());
    }
}
