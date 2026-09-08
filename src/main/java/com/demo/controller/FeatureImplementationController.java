package com.demo.controller;

import com.demo.model.FeatureImplementation;
import com.demo.repository.FeatureImplementationRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/featureimplementations")
@CrossOrigin(origins = "*")
public class FeatureImplementationController {

    private final FeatureImplementationRepository repository;

    @Autowired
    public FeatureImplementationController(FeatureImplementationRepository repository) {
        this.repository = repository;
    }

    @GetMapping
    public List<FeatureImplementation> getAll() {
        return repository.findAll();
    }

    @GetMapping("/{id}")
    public ResponseEntity<FeatureImplementation> getById(@PathVariable Long id) {
        return repository.findById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping
    public ResponseEntity<FeatureImplementation> create(@Validated @RequestBody FeatureImplementation item) {
        FeatureImplementation saved = repository.save(item);
        return ResponseEntity.status(HttpStatus.CREATED).body(saved);
    }

    @PatchMapping("/{id}")
    public ResponseEntity<FeatureImplementation> patch(@PathVariable Long id, @RequestBody FeatureImplementation updates) {
        return repository.findById(id).map(existing -> {
            if (updates.getName() != null) existing.setName(updates.getName());
            if (updates.getDescription() != null) existing.setDescription(updates.getDescription());
            FeatureImplementation updated = repository.save(existing);
            return ResponseEntity.ok(updated);
        }).orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable Long id) {
        if (!repository.existsById(id)) {
            return ResponseEntity.notFound().build();
        }
        repository.deleteById(id);
        return ResponseEntity.noContent().build();
    }
}
